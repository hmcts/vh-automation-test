using UI.PageModels.Pages.Video.Participant;
using VideoApi.Contract.Enums;
using VideoApi.Contract.Responses;

namespace UI.NUnitVersion.Video;

public class EndToEndTest : VideoWebUiTest
{
    private string _hearingIdString;
    private ConferenceDetailsResponse _conference;
    private JusticeUserResponse _justiceUser;

    [Test]
    [Category("Daily")]
    [Category("a11y")]
    [Category("Smoke Test")]
    [Description("Book a hearing. " +
                 "Allocate to a CSO. Log in as a judge, edit their display name, and log in as 4 participants. " +
                 "Log in as a VHO and monitor the changes. IM between VHO and Judge.")]
    public async Task EndToEnd()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDto(HearingTestData.Judge, scheduledDateTime:hearingScheduledDateAndTime);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        
        await BookHearing(hearingDto);

        //Login
        var vhoVenueSelectionPage = LoginAsVho(HearingTestData.VhOfficerUsername, EnvConfigSettings.UserPassword);
        ParticipantDrivers[HearingTestData.VhOfficerUsername].VhVideoWebPage = vhoVenueSelectionPage;
 
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
        var ccHearingPanel = commandCentrePage.ClickHearingsButton();
        var testParticipant = _conference.Participants.First(e => e.UserRole == UserRole.Individual);
        ccHearingPanel.ValidateParticipantStatusBeforeLogin(testParticipant.Id);
        
        // loop through all participants in hearing and login as each one
        foreach (var participant in hearingDto.Participants)
        {
            var participantUsername = participant.Username;
            var participantPassword = EnvConfigSettings.UserPassword;
            var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative);
            var participantWaitingRoom = participantHearingList
                .SelectHearing(_conference.Id).GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
            // store the participant driver in a dictionary so we can access it later to sign out
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        }

        var testParticipantWaitingRoom = (ParticipantWaitingRoomPage)ParticipantDrivers[testParticipant.Username].VhVideoWebPage;
        ccHearingPanel.ValidateParticipantStatusAfterLogin(testParticipant.Id, _conference.Id.ToString());
        
        // VHO will be able to see participants moving from waiting room to consultation room and hearing room
        var ccMessagingPanel = commandCentrePage.ClickMessagesButton();
        ccMessagingPanel.ValidateInstantMessagingOutboundScenario(testParticipant.DisplayName);
            
        // VHO will be able to receive a reply to the IM
        var messageToCso = "Hello from the participant";
        testParticipantWaitingRoom
            .OpenChatWithVHO()
            .SendAMessageToVHO(messageToCso);
        ccMessagingPanel.ValidateInstantMessagingInboundScenario(messageToCso);

        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(_conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        // edit the judge display name
        judgeWaitingRoomPage.EditJudgeDisplayName();

        // confirm all participants are connected
        judgeWaitingRoomPage.GetParticipantConnectedCount().Should().Be(hearingDto.Participants.Count);
        
        // VHO will be able to monitor HearingStatus updates
        ccHearingPanel = ccMessagingPanel.ClickHearingsButton();
        ccHearingPanel.ValidateHearingStatusBeforeStartScenario();
        
        // start hearing
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        ccHearingPanel.ValidateLiveHearingStatusScenario(_conference.Id.ToString());
        
        // pause hearing
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        ccHearingPanel.ValidatePausedHearingStatusScenario(_conference.Id.ToString());
        
        // resume then close hearing
        judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();

        // sign out of each hearing

        SignOutAllUsers();
        Assert.Pass();
    }

    private async Task BookHearing(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
            
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();

        var summaryPage = createHearingPage.BookAHearingJourney(bookingDto, FeatureToggles.UseV2Api());
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestHearingIds.Add(_hearingIdString);
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        bookingDetailsPage.GoToDashboardPage();
        
        var teamMemberUsername = WorkAllocationTestData.JusticeUserUsername;
        
        _justiceUser = await CreateVhTeamLeaderJusticeUserIfNotExist(teamMemberUsername);
        _conference = await VideoApiClient.GetConferenceByHearingRefIdAsync(new Guid(_hearingIdString) , false);
        
        var manageWorkAllocationPage = dashboardPage.GoToManageWorkAllocation();
        
        manageWorkAllocationPage.AllocateJusticeUserToHearing(
            caseNumber: _conference.CaseNumber,
            justiceUserDisplayName: _justiceUser.FullName,
            justiceUserUsername: _justiceUser.Username);
    }
}