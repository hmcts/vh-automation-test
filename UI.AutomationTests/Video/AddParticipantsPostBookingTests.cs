using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Video;

public class AddParticipantsPostBookingTests : VideoWebUiTest
{
    private string _hearingIdString;
    
    [Description("Book a hearing with a judge and add a participant after booking. Check if the notification appears and if the user joins the judge in the hearing after a hearing is started.")]
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
        
        
        //log in as a panelMember, go tp waiting room and wait for alerts
        var panelMemberUsername = hearingDto.PanelMembers[0].Username;
        var panelMemberHearingListPage = LoginAsPanelMember(panelMemberUsername, EnvConfigSettings.UserPassword);
        var panelMemberWaitingRoomPage = panelMemberHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[panelMemberUsername].VhVideoWebPage = panelMemberWaitingRoomPage;
                   
        var participantsToAdd = new List<BookingParticipantDto>(){HearingTestData.KnownParticipantsForTesting()[0]};
        var confirmationPage = bookingDetailsPage.AddParticipantsToBooking(participantsToAdd);
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        hearingDto.Participants.AddRange(participantsToAdd);
        
        // start a hearing and late joiners should be transferred to the hearing
        judgeWaitingRoomPage.ClearParticipantAddedNotification();
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeHearingRoomPage.WaitForCountdownToComplete();

        
        // loop through all participants in hearing and login as each one
        Parallel.ForEach(hearingDto.Participants, participant =>
        {
            var participantUsername = participant.Username;
            var participantPassword = EnvConfigSettings.UserPassword;
            var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative, participant.VideoFileName);
            var participantWaitingRoom = participantHearingList
                .SelectHearing(conference.Id)
                .GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
            // store the participant driver in a dictionary so we can access it later to sign out
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        });
        
        judgeHearingRoomPage.IsParticipantInHearingAlreadyInSession(participantsToAdd[0].DisplayName).Should().BeTrue($"{participantsToAdd[0].DisplayName} should have been transferred to the hearing after joining a hearing in session");
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();

        Assert.Pass("""
                    Booked a hearing with a judge, added a participant after booking, 
                    and checked if the notification appears and if the user joins the judge in the hearing 
                    after a hearing has already started.
                    """);
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
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        return confirmationPage.ClickViewBookingLink();
    }
    
}