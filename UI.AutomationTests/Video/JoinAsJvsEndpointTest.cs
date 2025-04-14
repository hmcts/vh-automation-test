using UI.PageModels.Pages.PexipInfinityWeb;
using UI.PageModels.Pages.Video.Participant;
using VideoApi.Contract.Enums;
using VideoApi.Contract.Responses;

namespace UI.AutomationTests.Video;

public class JoinAsJvsEndpointTest : VideoWebUiTest
{
    [Test]
    [Category("video")]
    [Category("coreVideo")]
    public async Task LoginWithJvsWithLinkedParticipants()
    {
        // Arrange
        var hearing = await CreateTestHearing();
        TestHearingIds.Add(hearing.Id.ToString());
        
        await TestContext.Out.WriteLineAsync($"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");
        // wait until conference is created
        var conference = await GetConference(hearing.Id);
        
        // log in as JVS endpoint
        var endpoint = conference.Endpoints[0];
        var pexipNodeAddress = EnvConfigSettings.PexipNodeAddress;
        
        var displayName = "VH Test";
        var pexipWebAppUrl = PexipWebAppUrlBuilder.BuildPexipWebAppUrl(pexipNodeAddress, endpoint.SipAddress, endpoint.Pin, displayName);
        var jvsWebPage = LoginAsJvsEndpoint(pexipWebAppUrl, endpoint.DisplayName);
        jvsWebPage.ClickJoinMeetingButton();
        
        var intermediary = conference.Participants
            .First(cp => hearing.Participants
                .Any(hp => hp.Username == cp.Username && hp.HearingRoleCode == HearingTestData.HearingRoleCodes.Intermediary));
        
        var representative = conference.Participants
            .First(cp => hearing.Participants
                .Any(hp => hp.Username == cp.Username && hp.HearingRoleCode == HearingTestData.HearingRoleCodes.Representative));

        var otherParticipant = conference.Participants.First(e => e.UserRole == UserRole.Individual);
        
        // log in as participants and go to waiting room
        var repWaitingRoom = LoginInAsParticipantToWaitingRoomJourney(conference, representative.Username, HearingTestData.Representative01FileName);
        var intWaitingRoom = LoginInAsParticipantToWaitingRoomJourney(conference, intermediary.Username, HearingTestData.Representative02FileName);
        var otherParticipantRoom = LoginInAsParticipantToWaitingRoomJourney(conference, otherParticipant.Username, HearingTestData.Individual01FileName);
        var judgeWaitingRoomPage = LoginWithJudge(hearing, conference);
        
        ValidatePrivateConsultationLogicWithEndpoint(endpoint, 
            representative: representative, 
            otherParticipant: otherParticipant,
            intermediary: intermediary, 
            otherParticipantRoom: otherParticipantRoom, 
            intWaitingRoom: intWaitingRoom, 
            repWaitingRoomPage: repWaitingRoom);

        judgeWaitingRoomPage.WaitForParticipantToBeConnected(endpoint.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnectedById(representative.Id.ToString());
        judgeWaitingRoomPage.WaitForParticipantToBeConnectedById(intermediary.Id.ToString());
        judgeWaitingRoomPage.WaitForParticipantToBeConnectedById(otherParticipant.Id.ToString());
        
        //Test the Hearing
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeHearingRoomPage.WaitForCountdownToComplete();
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.Id).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(representative.Id).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(intermediary.Id).Should().BeTrue();

        var participantHearingRoom = repWaitingRoom.TransferToHearingRoom();

        judgeHearingRoomPage.DismissParticipant(endpoint.DisplayName, endpoint.Id.ToString());
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.Id).Should().BeFalse();
        
        judgeHearingRoomPage.DismissParticipant(representative.DisplayName, representative.Id.ToString());
        judgeHearingRoomPage.IsParticipantInHearing(representative.Id).Should().BeFalse();
        
        // triggers the ctor to check the page has loaded correctly
        _ = participantHearingRoom.TransferToWaitingRoom();
        
        judgeHearingRoomPage.AdmitParticipant(endpoint.DisplayName, endpoint.Id.ToString());
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.Id).Should().BeTrue();
        
        judgeHearingRoomPage.AdmitParticipant(representative.DisplayName, representative.Id.ToString());
        judgeHearingRoomPage.IsParticipantInHearing(representative.Id).Should().BeTrue();
        
        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();
        
        judgeWaitingRoomPage.GetConsultationCloseTime().Should()
            .MatchRegex(@"The consultation room will close at \d{2}:\d{2}");
        
        Assert.Pass("Logged in as JVS endpoint and connected to the hearing");
    }

    private JudgeWaitingRoomPage LoginWithJudge(HearingDetailsResponseV2 hearing, ConferenceDetailsResponse conference)
    {
        // log in as judge and start the hearing
        var judgeUsername = hearing.JudicialOfficeHolders[0].Email;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        return judgeWaitingRoomPage;
    }

    private void ValidatePrivateConsultationLogicWithEndpoint(
        EndpointResponse endpoint, 
        ParticipantResponse representative, 
        ParticipantResponse otherParticipant,
        ParticipantResponse intermediary, 
        ParticipantWaitingRoomPage repWaitingRoomPage,
        ParticipantWaitingRoomPage intWaitingRoom,
        ParticipantWaitingRoomPage otherParticipantRoom)
    {
        //Start Private Consultation with JVS
        var intConsultationRoom = intWaitingRoom.StartPrivateConsultation([endpoint.DisplayName]);
        intConsultationRoom.IsParticipantConnected(endpoint.DisplayName).Should().BeTrue();
        
        //Invite other linked participant
        intConsultationRoom.InviteParticipant(representative.DisplayName);
        repWaitingRoomPage.AcceptPrivateConsultation();
        intConsultationRoom.IsParticipantConnected(representative.DisplayName).Should().BeTrue();
        var repConsultationRoom = new ConsultationRoomPage(ParticipantDrivers[representative.Username].Driver.GetDriver(), 60);
        
        //Invite a 3rd party participant
        intConsultationRoom.InviteParticipant(otherParticipant.DisplayName);
        otherParticipantRoom.AcceptPrivateConsultation();
        intConsultationRoom.IsParticipantConnected(otherParticipant.DisplayName).Should().BeTrue();
        var otherConsultationRoom = new ConsultationRoomPage(ParticipantDrivers[otherParticipant.Username].Driver.GetDriver(), 60);
        
        //leave and validate consultation endpoint is still in there with the other linked participant 
        intConsultationRoom.LeaveConsultationRoom();
        repConsultationRoom.IsParticipantConnected(intermediary.DisplayName).Should().BeFalse();
        repConsultationRoom.IsParticipantConnected(endpoint.DisplayName).Should().BeTrue();
        repConsultationRoom.IsParticipantConnected(otherParticipant.DisplayName).Should().BeTrue();
        
        //other linked participant to leave consultation, validate endpoint is also remove, via the 3rd party
        repConsultationRoom.LeaveConsultationRoom();
        otherConsultationRoom.IsParticipantConnected(representative.DisplayName).Should().BeFalse();
        otherConsultationRoom.IsParticipantConnected(endpoint.DisplayName).Should().BeFalse();
    }

    private ParticipantWaitingRoomPage LoginInAsParticipantToWaitingRoomJourney(ConferenceDetailsResponse conference, string username, string videoFile)
    {
        var participantHearingList = LoginAsParticipant(username, EnvConfigSettings.UserPassword, true, videoFile);
        var participantWaitingRoom = participantHearingList
            .SelectHearing(conference.Id).GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules()
            .AcceptDeclaration();
        // store the participant driver in a dictionary, so we can access it later to sign out
        ParticipantDrivers[username].VhVideoWebPage = participantWaitingRoom;
        return participantWaitingRoom;
    }

    private async Task<HearingDetailsResponseV2> CreateTestHearing()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateNewRequestDtoJudgeAndEndpointWithLinkedParticipants(scheduledDateTime: hearingScheduledDateAndTime);
        return await BookingsApiClient.BookNewHearingWithCodeAsync(hearingDto);
    }
}