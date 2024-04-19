using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking
{
    public class MultiDayHearingTest : AdminWebUiTest
    {
        protected BookingDetailsPage BookMultiDayHearingAndGoToDetailsPage(BookingDto bookingDto)
        {
            TestContext.WriteLine(
                $"Attempting to book a hearing with the case name: {bookingDto.CaseName} and case number: {bookingDto.CaseNumber}");
            
            var driver = VhDriver.GetDriver();
            driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
            var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
            var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
            
            var createHearingPage = dashboardPage.GoToBookANewHearing();
            var summaryPage = createHearingPage.BookAHearingJourney(bookingDto, FeatureToggles.UseV2Api(), isMultiDay: true);
            var confirmationPage = summaryPage.ClickBookButton();
            
            TestHearingIds.Add(confirmationPage.GetNewHearingId());

            var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
            return bookingDetailsPage;
        }
        
        /// <summary>
        /// Multi day hearings include the day number in their case name
        /// </summary>
        /// <param name="bookingDto"></param>
        /// <param name="numberOfDays"></param>
        protected static void UpdateCaseName(BookingDto bookingDto, int numberOfDays)
        {
            bookingDto.CaseName += $" Day 1 of {numberOfDays + 1}";
        }
        
        protected static DateTime GetFirstDayOfNextWeek(DateTime currentDate)
        {
            // Calculate the number of days until the next Monday
            var daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;
        
            // Add the days until the next Monday to the current date to get the first day of next week
            var firstDayOfNextWeek = currentDate.AddDays(daysUntilNextMonday);
        
            return firstDayOfNextWeek;
        }
    }
}
