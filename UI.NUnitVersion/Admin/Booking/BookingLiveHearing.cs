namespace UI.NUnitVersion.Admin.Booking;

public class BookingLiveHearing : AdminWebUiTest
{
    
    [Test]
    public void BookLiveHearing()
    {
        var date = DateTime.Now.AddMinutes(30); // Add 30 minutes to the current date and time
        var bookingDto = HearingTestData.CreateHearingDto(scheduledDateTime: date);


        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();

        var createHearingPage = dashboardPage.GoToBookANewHearing();

        createHearingPage.EnterHearingDetails(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType,
            bookingDto.HearingType);

        var hearingSchedulePage = createHearingPage.GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(bookingDto.ScheduledDateTime, bookingDto.DurationHour,
            bookingDto.DurationMinute, bookingDto.VenueName, bookingDto.RoomName);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails("auto_aw.judge_02@hearings.reform.hmcts.net", "Auto Judge", "");

        var addParticipantPage = assignJudgePage.GoToParticipantsPage();
        addParticipantPage.AddExistingParticipants(bookingDto.Participants);

        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();

        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation("This is a test info");

        var summaryPage = otherInformationPage.GoToSummaryPage();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.ClickViewBookingLink();

        dashboardPage = confirmationPage.GoToDashboardPage();

        var postBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var postBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var postBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var postBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        postBookingUnallocatedHearingsToday.Should().Be(preBookingUnallocatedHearingsToday);
        postBookingUnallocatedHearingsTomorrow.Should().BeGreaterThan(preBookingUnallocatedHearingsTomorrow);
        postBookingUnallocatedHearingsNextSevenDays.Should().BeGreaterThan(preBookingUnallocatedHearingsNextSevenDays);
        postBookingUnallocatedHearingsNextThirtyDays.Should()
            .BeGreaterThan(preBookingUnallocatedHearingsNextThirtyDays);
        
        dashboardPage.SignOut();

        Assert.Pass();
    }
}
    

