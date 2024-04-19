using UI.PageModels.Constants;

namespace UI.AutomationTests.Admin.Booking
{
    public class CancelMultiDayHearingTests : MultiDayHearingTest
    {
        [Category("Daily")]
        [Test]
        public void CancelThisAndUpcomingDaysOfMultiDayHearing()
        {
            var multiDayBookingEnhancementsEnabled = FeatureToggles.MultiDayBookingEnhancementsEnabled();
            if (!multiDayBookingEnhancementsEnabled)
                Assert.Ignore("Multi day booking enhancements are not both enabled, cannot cancel this and upcoming hearings. Skipping Test");
            
            const int numberOfDays = 3;
            var scheduledDateTime = GetFirstDayOfNextWeek(DateTime.UtcNow).Date.AddHours(10).AddMinutes(0);
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, scheduledDateTime);
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            bookingDetailsPage.ClickCancelBooking();
            bookingDetailsPage.CancelThisAndUpcomingDays(CancellationReasons.EquipmentIncompatible);
  
            bookingDetailsPage.ValidateBookingIsCancelled();
            // Return to the booking list and validate the details page for each of the subsequent days in the multi-day hearing
            var driver = VhDriver.GetDriver();
            var hearingDates = ExtractDatesForMultiDayHearing(hearingDto)
                .Where(d => d > hearingDto.ScheduledDateTime)
                .ToList();
            foreach (var date in hearingDates)
            {
                hearingDto.ScheduledDateTime = date;
                SearchAndValidateHearing(driver, hearingDto);
            }
            Assert.Pass();
        }
        
        private void SearchAndValidateHearing(IWebDriver driver, BookingDto hearingDto)
        {
            // Search for the hearing on the booking list page
            driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
            var dashboardPage = new DashboardPage(driver, EnvConfigSettings.DefaultElementWait);
            var bookingListPage = dashboardPage.GoToBookingList();
            var queryDto = new BookingListQueryDto
            {
                CaseNumber = hearingDto.CaseNumber,
                StartDate = hearingDto.ScheduledDateTime
            };
            bookingListPage.SearchForBooking(queryDto);
            
            // Validate the details
            var bookingDetailPage = bookingListPage.ViewBookingDetails(queryDto.CaseNumber);
            bookingDetailPage.ValidateBookingIsCancelled();
        }

        private static IEnumerable<DateTime> ExtractDatesForMultiDayHearing(BookingDto hearingDto)
        {
            var dates = new List<DateTime>();
            var startDate = hearingDto.ScheduledDateTime;
            var endDate = hearingDto.EndDateTime;
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }
    }
}
