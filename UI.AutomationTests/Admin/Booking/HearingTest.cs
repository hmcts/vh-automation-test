using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking
{
    public abstract class HearingTest : AdminWebUiTest
    {
        protected BookingDetailsPage BookHearingAndGoToDetailsPage(BookingDto bookingDto)
        {
            TestContext.Out.WriteLine(
                $"Attempting to book a hearing with the case name: {bookingDto.CaseName} and case number: {bookingDto.CaseNumber}");
            
            var driver = VhDriver.GetDriver();
            driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
            var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
            var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

            var createHearingPage = dashboardPage.GoToBookANewHearing();
            var summaryPage = createHearingPage.BookAHearingJourney(bookingDto);
            var confirmationPage = summaryPage.ClickBookButton();
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            TestHearingIds.Add(confirmationPage.GetNewHearingId());
            var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
            return bookingDetailsPage;
        }
    }
}
