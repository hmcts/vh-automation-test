using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Video;

public class AddParticipantsPostBookingTests : VideoWebUiTest
{
    private string _hearingIdString;
    
    [Description("Book a hearing with a judge and add a participant after booking. Check if the notification appears and if the user joins the judge in the hearing when a hearing is started.")]
    [Test]
    [Category("video")]
    public async Task should_add_new_participant_after_booking()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDtoWithOnlyAJudge(scheduledDateTime:hearingScheduledDateAndTime);
        await TestContext.Out.WriteLineAsync(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);
        var conference = await GetConference(new Guid(_hearingIdString));

        // log in as judge, go to waiting room and wait for alerts
        var judgeUsername = hearingDto.Judge.Username;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        var participantsToAdd = new List<BookingParticipantDto>(){HearingTestData.KnownParticipantsForTesting()[0]};
        var confirmationPage = bookingDetailsPage.AddParticipantsToBooking(participantsToAdd);
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        hearingDto.Participants.AddRange(participantsToAdd);
        
        var participantsInConference = await VideoApiClient.GetParticipantsByConferenceIdAsync(conference.Id);
        // loop through all participants in hearing and login as each one
        foreach (var participant in hearingDto.Participants)
        {
            var participantUsername = participant.Username;
            var participantPassword = EnvConfigSettings.UserPassword;
            var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative);
            var participantWaitingRoom = participantHearingList
                .SelectHearing(conference.Id)
                .GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
            // store the participant driver in a dictionary so we can access it later to sign out
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
            
            judgeWaitingRoomPage.WaitForParticipantToBeConnected(participant.FullName);
            judgeWaitingRoomPage.ClearParticipantAddedNotification();
        }

        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        foreach (var participant in participantsToAdd)
        {
            var p = participantsInConference.First(x => x.Username == participant.Username);
            judgeHearingRoomPage.AdmitParticipant(p.DisplayName, p.Id.ToString());
        }
        
        judgeHearingRoomPage.WaitForCountdownToComplete();
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();

        Assert.Pass();
    }
    
    private BookingDetailsPage BookHearingAndGoToDetailsPage(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();
        var summaryPage = createHearingPage.BookAHearingJourney(bookingDto);
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestHearingIds.Add(_hearingIdString);
        TestContext.Out.WriteLine($"Hearing  ID: {_hearingIdString}");
        return confirmationPage.ClickViewBookingLink();
    }
}