namespace UI.AutomationTests.Admin.Booking;

public class CreatingMultiDayHearingTests : AdminWebUiTest
{
    private BookingDto _bookingDto;
    private string _groupId;

    
    [Test]
    [Category("admin")]
    public void CreatingMultiDayHearing()
    {
        var numberOfDays = 7;
        _bookingDto = HearingTestData.CreateMultiDayDto(numberOfDays, DateTime.Today.AddDays(1).AddHours(9).AddMinutes(00));
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        
        createHearingPage.EnterHearingDetails(_bookingDto);
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        //Create Multi-day schedule
        hearingSchedulePage.EnterMultiDayHearingSchedule(_bookingDto);
        
        //Add Judge
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge);
        
        //Add Participants
        var addParticipantPage = assignJudgePage.GotToNextPage();
        addParticipantPage.AddParticipants(_bookingDto.Participants);
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);
        
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryPage(_bookingDto, true);
        
        var confirmationPage = summaryPage.ClickBookButton();
        _groupId = confirmationPage.GetNewHearingId();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        
        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);
        dashboardPage = confirmationPage.GoToDashboardPage();
        
        var postBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var postBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var postBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var postBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        postBookingUnallocatedHearingsToday.Should().BeGreaterOrEqualTo(preBookingUnallocatedHearingsToday);
        postBookingUnallocatedHearingsTomorrow.Should().BeGreaterThan(preBookingUnallocatedHearingsTomorrow);
        postBookingUnallocatedHearingsNextSevenDays.Should().BeGreaterThan(preBookingUnallocatedHearingsNextSevenDays);
        postBookingUnallocatedHearingsNextThirtyDays.Should().BeGreaterThan(preBookingUnallocatedHearingsNextThirtyDays);
        
        dashboardPage.SignOut();

        Assert.Pass();
    }

    [TearDown]
    public async Task CleanUpMultiDayHearings()
    {
        if(string.IsNullOrEmpty(_groupId))return;
        var hearings = await BookingsApiClient.GetHearingsByGroupIdAsync(Guid.Parse(_groupId));
        foreach (var hearingId in hearings.Select(h => h.Id))
            try
            {
                await BookingsApiClient.RemoveHearingAsync(hearingId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to remove hearing {hearingId} with exception {e.Message}");
            }
    }
}