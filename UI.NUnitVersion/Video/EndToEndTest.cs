namespace UI.NUnitVersion.Video;

public class EndToEndTest : VideoWebUiTest
{
    private string _hearingIdString;

    [Category("Daily")]
    [Test]
    [Category("a11y")]
    public void BookAHearingAndLogInAsJudgeAndParticipants()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDto(scheduledDateTime:hearingScheduledDateAndTime);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        
        BookHearing(hearingDto);

        // loop through all participants in hearing and login as each one
        foreach (var participant in hearingDto.Participants)
        {
            var participantUsername = participant.Username;
            var participantPassword = EnvConfigSettings.UserPassword;
            var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative);
            var participantWaitingRoom = participantHearingList.SelectHearing(hearingDto.CaseName).GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration();
            // store the participant driver in a dictionary so we can access it later to sign out
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        }

        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(hearingDto.CaseName);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;

        // confirm all participants are connected
        judgeWaitingRoomPage.GetParticipantConnectedCount().Should().Be(hearingDto.Participants.Count);

        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();

        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();

        // sign out of each hearing
        SignOutAllUsers();
        
        Assert.Pass();
    }

    private void BookHearing(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();

        var summaryPage = createHearingPage.EnterHearingDetails(bookingDto);
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        confirmationPage.ClickViewBookingLink();
    }
    
    protected override async Task CleanUp()
    {
        if(_hearingIdString != null)
        {
            var hearingId = Guid.Parse(_hearingIdString);
            await BookingsApiClient.RemoveHearingAsync(hearingId);
            TestContext.WriteLine($"Removed Hearing {hearingId}");
            _hearingIdString = null;
        }
    }
}