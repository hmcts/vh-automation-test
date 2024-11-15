namespace UI.AutomationTests.Admin.Booking;

public class CreatingMultiDayHearingIndividualDateTests : AdminWebUiTest
{
    private BookingDto _bookingDto;
    private string _groupId;

    
    [Test]
    [Category("admin")]
    public void CreatingMultiDayHearingIndividualDate()
    {
        var numberOfDays = 7;
        _bookingDto = HearingTestData.CreateMultiDayDto(numberOfDays, DateTime.Today.AddDays(1).AddHours(9).AddMinutes(00));
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        
        createHearingPage.EnterHearingDetails(_bookingDto);
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        //Create Multi-day schedule
        hearingSchedulePage.EnterMultiDayHearingScheduleWithIndividualDates(_bookingDto, out var individualDatesForValidation);
        
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
        summaryPage.ValidateSummaryPage(_bookingDto, true, individualDatesForValidation);
        
        var confirmationPage = summaryPage.ClickBookButton();
        _groupId = confirmationPage.GetNewHearingId();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        
        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);
        dashboardPage = confirmationPage.GoToDashboardPage();
        
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