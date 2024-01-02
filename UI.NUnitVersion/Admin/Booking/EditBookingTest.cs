using UI.PageModels.Pages.Admin.Booking;

namespace UI.NUnitVersion.Admin.Booking;

public class EditBookingTest : AdminWebUiTest
{
    private string _hearingIdString;

    [Test]
    public void should_update_booking_schedule()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(60);
        var hearingDto = HearingTestData.CreateHearingDtoWithEndpoints(judgeUsername:"auto_aw.judge_02@hearings.reform.hmcts.net",scheduledDateTime:hearingScheduledDateAndTime);
        
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);

        var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
        var confirmationPage =
            bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
        
        confirmationPage.IsBookingSuccessful().Should().BeTrue();

        Assert.Pass();
    }
    
    private BookingDetailsPage BookHearingAndGoToDetailsPage(BookingDto bookingDto)
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();
        var summaryPage = createHearingPage.EnterHearingDetails(bookingDto);
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestContext.WriteLine($"Hearing  ID: {_hearingIdString}");
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        return bookingDetailsPage;
    }
    
    protected override async Task CleanUp()
    {
        if(_hearingIdString != null)
        {
            var hearingId = Guid.Parse(_hearingIdString);
            TestContext.WriteLine($"Removing Hearing {hearingId}");
            await BookingsApiClient.RemoveHearingAsync(hearingId);
            _hearingIdString = null;
        }
    }
    
}