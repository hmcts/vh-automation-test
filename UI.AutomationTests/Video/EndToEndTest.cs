using BookingsApi.Contract.V1.Responses;
using UI.Common.Utilities;
using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.Vho;
using UI.PageModels.Pages.Video.Vho.DashboardCommandCentre;
using VideoApi.Contract.Enums;
using VideoApi.Contract.Responses;
using ParticipantResponse = VideoApi.Contract.Responses.ParticipantResponse;

namespace UI.AutomationTests.Video;

public class EndToEndTest : VideoWebUiTest
{
    private string _hearingIdString;
    private ConferenceDetailsResponse _conference;
    private JusticeUserResponse _justiceUser;
    
    
    [Test]
    [Category("a11y")]
    [Category("video")]
    [Category("regression")]
    [Description("Book a hearing." +
                 "Allocate to a CSO." +
                 "Log in as a judge, edit their display name, and log in as 4 participants." +
                 "Log in as a panelMember"+
                 "Log in as a VHO and monitor the changes. IM between VHO and Judge." +
                 "Start and stop hearing. Log out with all users. ")]
    public async Task EndToEnd()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDto(HearingTestData.JudgePersonalCode, HearingTestData.JudgeUsername, scheduledDateTime: hearingScheduledDateAndTime);
        var newUser = HearingTestData.CreateNewParticipantDto();
        hearingDto.NewParticipants.Add(newUser);
        await TestContext.Out.WriteLineAsync(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}"); await BookHearing(hearingDto);
        
        //Login
        var vhoVenueSelectionPage = LoginAsVho(HearingTestData.VhOfficerUsername, EnvConfigSettings.UserPassword);
        ParticipantDrivers[HearingTestData.VhOfficerUsername].VhVideoWebPage = vhoVenueSelectionPage;
        var commandCentrePage = CsoCommandCentreJourney(vhoVenueSelectionPage, hearingDto, out var ccHearingPanel, out var testParticipant);

        // loop through all participants in hearing and login as each one
        Parallel.ForEach(hearingDto.Participants, ParticipantLoginToWaitingRoomJourney);
        
        // login with new user with temp password journey
        var newUserTempPassword = await EmailNotificationService.GetTempPasswordForUser(newUser.ContactEmail, hearingDto.CaseName);
        NewParticipantLoginToWaitingRoom(newUser, newUserTempPassword);
        hearingDto.Participants.Add(newUser);

        var testParticipantWaitingRoom = (ParticipantWaitingRoomPage)ParticipantDrivers[testParticipant.Username].VhVideoWebPage;
        ccHearingPanel.ValidateParticipantStatusAfterLogin(testParticipant.Id, _conference.Id.ToString());
        
        if(FeatureToggle.Instance().IMEnabled())
            AssertInstantMessagesFeature(commandCentrePage, testParticipant, testParticipantWaitingRoom);

        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing (_conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        // log in as PanelMember and enter the consultation 
        var panelMemberUsername = hearingDto.PanelMembers[0].Username;
        var panelMemberPassword = EnvConfigSettings.UserPassword;
        var panelMemberHearingListPage = LoginAsPanelMember (panelMemberUsername, panelMemberPassword);
        var panelMemberWaitingRoomPage = panelMemberHearingListPage.SelectHearing(_conference.Id);
        ParticipantDrivers[panelMemberUsername].VhVideoWebPage = panelMemberWaitingRoomPage;
        
        // edit the judge display name
        judgeWaitingRoomPage.EditJudgeDisplayName();

        // confirm all participants are connected
        judgeWaitingRoomPage.GetParticipantConnectedCount().Should().Be(hearingDto.Participants.Count + hearingDto.PanelMembers.Count);
        
        // VHO will be able to monitor HearingStatus updates
        ccHearingPanel.ClickHearingsButton();
        ccHearingPanel.ValidateHearingStatusBeforeStartScenario();
        
        // start hearing
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        ccHearingPanel.ValidateLiveHearingStatusScenario(_conference.Id.ToString());
        
        // pause hearing
        judgeHearingRoomPage.WaitForCountdownToComplete(buffer: 10);
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        ccHearingPanel.ValidatePausedHearingStatusScenario(_conference.Id.ToString());
        
        // resume then close hearing
        judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();
        // sign out of each hearing
        SignOutAllUsers();

        ReportAccessibility();
        Assert.Pass();
    }

    private void ParticipantLoginToWaitingRoomJourney(BookingParticipantDto participant)
    {
        var participantUsername = participant.Username;
        var participantPassword = EnvConfigSettings.UserPassword;
        var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative, participant.VideoFileName);
        JourneyToWaitingRoom(participant, participantHearingList, participantUsername);
    }
    
    private void JourneyToWaitingRoom(BookingParticipantDto participant, ParticipantHearingListPage participantHearingList, string participantUsername)
    {
        var participantWaitingRoom = participantHearingList
            .SelectHearing(_conference.Id).GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
        // store the participant driver in a dictionary, so we can access it later to sign out
        ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
    }

    private void NewParticipantLoginToWaitingRoom(BookingParticipantDto participant, string tempPassword)
    {
        var participantUsername = participant.Username;
        var participantHearingList = LoginAsParticipant(participantUsername, tempPassword, participant.Role == GenericTestRole.Representative, participant.VideoFileName, isNew: true);
        JourneyToWaitingRoom(participant, participantHearingList, participantUsername);
    }
    private void PanelMemberLoginToWaitingRoomJourney(BookingParticipantDto panelMember)

    {
        var panelMemberUsername = panelMember.Username;
        var panelMemberPassword = EnvConfigSettings.UserPassword;
        var panelHearingListPage = LoginAsPanelMember(panelMemberUsername, panelMemberPassword);
        var panelMemberWaitingRoom = panelHearingListPage.SelectHearing(_conference.Id);
        ParticipantDrivers[panelMemberUsername].VhVideoWebPage = panelMemberWaitingRoom;
    }
    
    private CommandCentrePage CsoCommandCentreJourney(VhoVenueSelectionPage vhoVenueSelectionPage, BookingDto hearingDto,
        out CommandCentreHearing ccHearingPanel, out ParticipantResponse testParticipant)
    {
        // vho will be able to see hearings at selected venue(s) OR selected vho(s)
        // // VHO will go back to venue selection then select a hearing to monitor based on the allocated CSO
        var commandCentrePage = vhoVenueSelectionPage.SelectHearingsByVenues(hearingDto.VenueName);
        vhoVenueSelectionPage = commandCentrePage.ChangeVenueSelection();
        commandCentrePage = vhoVenueSelectionPage.SelectHearingsByAllocatedCso(_justiceUser);
        
        // Hearings will be displayed in chronological order as per VIH-10224
        commandCentrePage.ValidateHearingsAreInChronologicalOrder();
        
        // vho will be able to select a hearing to monitor
        commandCentrePage.SelectConferenceFromList(_conference.Id.ToString());
        
        // vho will be able to see connectivity status of participants in that hearing
        ccHearingPanel = commandCentrePage.ClickHearingsButton();
        testParticipant = _conference.Participants
            .First(e => e.UserRole == UserRole.Individual && !e.Username.Contains("new"));
        ccHearingPanel.ValidateParticipantStatusBeforeLogin(testParticipant.Id);
        return commandCentrePage;
    }

    private static void AssertInstantMessagesFeature(CommandCentrePage commandCentrePage,
        ParticipantResponse testParticipant, ParticipantWaitingRoomPage testParticipantWaitingRoom)
    {
        // VHO will be able to see participants moving from waiting room to consultation room and hearing room
        var ccMessagingPanel = commandCentrePage.ClickMessagesButton();
        ccMessagingPanel.ValidateInstantMessagingOutboundScenario(testParticipant.DisplayName);
            
        // VHO will be able to receive a reply to the IM
        var messageToCso = "Hello from the participant";
        testParticipantWaitingRoom
            .OpenChatWithVHO()
            .SendAMessageToVHO(messageToCso);
        ccMessagingPanel.ValidateInstantMessagingInboundScenario(messageToCso);
    }

    private async Task BookHearing(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
            
        await driver.Navigate().GoToUrlAsync(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();

        var summaryPage = createHearingPage.BookAHearingJourney(bookingDto);
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestHearingIds.Add(_hearingIdString);
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        bookingDetailsPage.GoToDashboardPage();
        
        var teamMemberUsername = WorkAllocationTestData.JusticeUserUsername;
        
        _justiceUser = await CreateVhTeamLeaderJusticeUserIfNotExist(teamMemberUsername);
        _conference = await GetConference(new Guid(_hearingIdString));
        
        var manageWorkAllocationPage = dashboardPage.GoToManageWorkAllocation();
        
        manageWorkAllocationPage.AllocateJusticeUserToHearing(
            caseNumber: bookingDto.CaseNumber,
            justiceUserDisplayName: _justiceUser.FullName,
            justiceUserUsername: _justiceUser.Username);
    }
}