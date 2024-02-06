
using UI.AutomationTests.TestData;
using UI.AutomationTests.Utilities;
using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking;

public class BookHearingNoJohTests : AdminWebUiTest
{
    private BookingDto _bookingDto;

    [Category("Daily")]
    [Test]
    public void BookAHearingNoJoh()
    {
        var v2Flag = FeatureToggles.UseV2Api();
        if(!FeatureToggles.EJudEnabled())
            Assert.Pass("Ejud is not enabled, will not be able to book without a judge. Skipping Test");
        
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDto(
            judgeUsername: HearingTestData.Judge, 
            scheduledDateTime: date);
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
        
        if(v2Flag)
            createHearingPage.EnterHearingDetailsV2(_bookingDto.CaseNumber, _bookingDto.CaseName, _bookingDto.CaseType);
        else
            createHearingPage.EnterHearingDetails(_bookingDto.CaseNumber, _bookingDto.CaseName, _bookingDto.CaseType, _bookingDto.HearingType);
        
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        hearingSchedulePage.EnterSingleDayHearingSchedule(
            _bookingDto.ScheduledDateTime, 
            _bookingDto.DurationHour,
            _bookingDto.DurationMinute, 
            _bookingDto.VenueName, 
            _bookingDto.RoomName);
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        
        //skipping assignment of judge
        var addParticipantPage = assignJudgePage.GotToNextPage(v2Flag);
        addParticipantPage.AddParticipants(_bookingDto.Participants.First());
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryNoJudgePage(_bookingDto);
        
        var confirmationPage = summaryPage.ClickBookButton();
        
        confirmationPage.ClickViewBookingLink();
        confirmationPage.EditHearing();
        confirmationPage.GotoJudgeAssignmentPage(v2Flag);

        if (v2Flag)
        {
            assignJudgePage.AssignPresidingJudiciaryDetails(_bookingDto.Judge.Username, _bookingDto.Judge.DisplayName);
            assignJudgePage.ClickSaveJudgeButton();
        }
        else
            assignJudgePage.EnterJudgeDetails(_bookingDto.Judge.Username, _bookingDto.Judge.DisplayName, _bookingDto.Judge.Phone);
        
        summaryPage = assignJudgePage.GotToNextPageOnEdit();
        summaryPage.ClickBookButton();
        
        TestHearingIds.Add(confirmationPage.GetNewHearingId());
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        
        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);
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