namespace UI.AutomationTests.Admin.Booking;

public class BookHearingTests : AdminWebUiTest
{
    private BookingDto _bookingDto;

    [Category("Daily")]
    [Test]
    public void BookAHearing()
    {
        var v2Flag = FeatureToggle.Instance().UseV2Api();
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDtoWithEndpoints(judgeUsername: HearingTestData.Judge, scheduledDateTime: date);
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        var newUser = HearingTestData.CreateNewParticipantDto();
        CreatedUsers.Add(newUser.Username);
        _bookingDto.NewParticipants.Add(newUser);
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails(_bookingDto, v2Flag);
        
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        hearingSchedulePage.EnterSingleDayHearingSchedule(_bookingDto);
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge, v2Flag);
            
        var addParticipantPage = assignJudgePage.GotToNextPage(v2Flag);
        
        addParticipantPage.AddAllParticipantsFromDto(_bookingDto);
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryPage(_bookingDto);
        
        var confirmationPage = summaryPage.ClickBookButton();
        TestHearingIds.Add(confirmationPage.GetNewHearingId());
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
}