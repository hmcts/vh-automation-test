using UI.Common.Utilities;

namespace UI.AutomationTests.Admin.Booking;

public class BookHearingNoJohTests : AdminWebUiTest
{
    private BookingDto _bookingDto;

    
    [Test]
    [Category("admin")]
    public void BookAHearingNoJoh()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDto(
            HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, 
            scheduledDateTime: date);
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails(_bookingDto);
        
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        hearingSchedulePage.EnterSingleDayHearingSchedule(_bookingDto);
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        
        //skipping assignment of judge
        var addParticipantPage = assignJudgePage.GotToNextPage();
        addParticipantPage.AddParticipants(_bookingDto.Participants);
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        var otherInformationPage = FeatureToggle.Instance().SpecialMeasuresEnabled()
            ? videoAccessPointsPage.GoToSpecialMeasuresPage().GoToOtherInformationPage()
            : videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryNoJudgePage(_bookingDto);
        
        var confirmationPage = summaryPage.ClickBookButton();
        
        confirmationPage.ClickViewBookingLink();
        confirmationPage.EditHearing();
        confirmationPage.GotoJudgeAssignmentPage();

        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge);
        
        summaryPage = assignJudgePage.GotToNextPageOnEdit();
        summaryPage.ClickBookButton();
        
        TestHearingIds.Add(confirmationPage.GetNewHearingId());
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        
        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);
        dashboardPage = confirmationPage.GoToDashboardPage();
        
        dashboardPage.SignOut();

        Assert.Pass("Hearing booked successfully without a judge.");
    }
}