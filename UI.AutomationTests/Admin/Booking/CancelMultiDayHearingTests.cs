using UI.PageModels.Constants;

namespace UI.AutomationTests.Admin.Booking
{
    public class CancelMultiDayHearingTests : MultiDayHearingTest
    {
        [Category("Daily")]
        [Test]
        public void CancelSingleDayOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var scheduledDateTime = GetFirstDayOfNextWeek(DateTime.UtcNow).Date.AddHours(10).AddMinutes(0);
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, scheduledDateTime);
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            bookingDetailsPage.ClickCancelBooking();
            bookingDetailsPage.CancelSingleHearing(CancellationReasons.EquipmentIncompatible);
  
            bookingDetailsPage.ValidateBookingIsCancelled();
            Assert.Pass();
        }
        
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
                bookingDetailsPage = SearchAndViewHearing(driver, hearingDto);
                bookingDetailsPage.ValidateBookingIsCancelled();
            }
            Assert.Pass();
        }
    }
}
