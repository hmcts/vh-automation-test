namespace UI.NUnitVersion.Admin.Booking;

[Category("Daily")]
public class BookHearingTests : AdminWebUiTest
{
    private BookingDto _bookingDto;
    
    [Test]
    public void BookAHearing()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDtoWithEndpoints(scheduledDateTime: date);

        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();

        var createHearingPage = dashboardPage.GoToBookANewHearing();

        createHearingPage.EnterHearingDetails(_bookingDto.CaseNumber, _bookingDto.CaseName, _bookingDto.CaseType,
            _bookingDto.HearingType);

        var hearingSchedulePage = createHearingPage.GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(_bookingDto.ScheduledDateTime, _bookingDto.DurationHour,
            _bookingDto.DurationMinute, _bookingDto.VenueName, _bookingDto.RoomName);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge.Username, _bookingDto.Judge.DisplayName, _bookingDto.Judge.Phone);

        var addParticipantPage = assignJudgePage.GoToParticipantsPage();
        addParticipantPage.AddExistingParticipants(_bookingDto.Participants);

        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);

        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);

        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryPage(_bookingDto);
        
        var confirmationPage = summaryPage.ClickBookButton();

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
    
    
    [Test]
    public void SearchForHearingViaBookingList()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        // ideally need to make this test run independently of other tests
        var bookingListPage = dashboardPage.GoToBookingList();
        var queryDto = new BookingListQueryDto()
        {
            CaseNumber = _bookingDto.CaseNumber,
            StartDate = _bookingDto.ScheduledDateTime.Date,
            EndDate = _bookingDto.ScheduledDateTime.Date,
            UnallocatedOnly = true
        };
        bookingListPage.SearchForBooking(queryDto);
        var bookingDetailsPage = bookingListPage.ViewBookingDetails(queryDto.CaseNumber);
        bookingDetailsPage.GetAllocatedTo().Should().Be("Not Allocated");
        bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl).Should().NotBeNullOrWhiteSpace();
        TestContext.WriteLine(bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl));
        dashboardPage.SignOut();
        Assert.Pass();
    }
}