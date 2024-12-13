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
    public async Task LoginWithJvsWithLinkedDefenceAdvocate()
    {
        // Arrange
        var hearing = await CreateTestHearing();
        TestHearingIds.Add(hearing.Id.ToString());
        
        await TestContext.Out.WriteLineAsync(
            $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");
        // wait until conference is created
        var conference = await GetConference(hearing.Id);
        
        // log in as JVS endpoint
        var endpoint = conference.Endpoints[0];
        var pexipNodeAddress = EnvConfigSettings.PexipNodeAddress;
        
        var displayName = "VH Test";
        var pexipWebAppUrl =
            PexipWebAppUrlBuilder.BuildPexipWebAppUrl(pexipNodeAddress, endpoint.SipAddress, endpoint.Pin, displayName);
        var jvsWebPage = LoginAsJvsEndpoint(pexipWebAppUrl, endpoint.DisplayName);
        jvsWebPage.ClickJoinMeetingButton();

        var representative = conference.Participants.First(x => x.UserRole == UserRole.Representative && endpoint.DefenceAdvocate == x.Username);
        var participantWaitingRoom = LoginInAsParticipantToWaitingRoomJourney(conference, representative);
        
        // log in as judge and start the hearing
        var judgeUsername = hearing.JudicialOfficeHolders[0].Email;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(endpoint.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnectedById(representative.Id.ToString());
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeHearingRoomPage.WaitForCountdownToComplete();
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.Id).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(representative.Id).Should().BeTrue();

        var participantHearingRoom = participantWaitingRoom.TransferToHearingRoom();

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

    private ParticipantWaitingRoomPage LoginInAsParticipantToWaitingRoomJourney(ConferenceDetailsResponse conference,
        ParticipantResponse representative)
    {

        var participantHearingList = LoginAsParticipant(representative.Username, EnvConfigSettings.UserPassword, true);
        var participantWaitingRoom = participantHearingList
            .SelectHearing(conference.Id).GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules()
            .AcceptDeclaration();
        // store the participant driver in a dictionary, so we can access it later to sign out
        ParticipantDrivers[representative.Username].VhVideoWebPage = participantWaitingRoom;
        return participantWaitingRoom;
    }

    private async Task<HearingDetailsResponseV2> CreateTestHearing()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateNewRequestDtoJudgeAndEndpointWithLinkedDa(scheduledDateTime: hearingScheduledDateAndTime);
        return await BookingsApiClient.BookNewHearingWithCodeAsync(hearingDto);
    }
}