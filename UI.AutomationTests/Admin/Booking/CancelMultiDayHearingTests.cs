using UI.Common.Utilities;
using UI.PageModels.Constants;

namespace UI.AutomationTests.Admin.Booking
{
    public class CancelMultiDayHearingTests : MultiDayHearingTest
    {
        
        [Test]
        public void CancelSingleDayOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var scheduledDateTime = GetFirstDayOfNextWeek(DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs)).Date
                .AddHours(10).AddMinutes(0);
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, scheduledDateTime);
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            bookingDetailsPage.ClickCancelBooking();
            bookingDetailsPage.CancelSingleHearing(CancellationReasons.EquipmentIncompatible);
  
            bookingDetailsPage.ValidateBookingIsCancelled();
            Assert.Pass();
        }
        
        
        [Test]
        [FeatureToggleSetting(FeatureToggle.MultiDayBookingEnhancementsToggleKey, true)]
        public void CancelThisAndUpcomingDaysOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var scheduledDateTime = GetFirstDayOfNextWeek(DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs)).Date
                .AddHours(10).AddMinutes(0);
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, scheduledDateTime);
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            bookingDetailsPage.ClickCancelBooking();
            bookingDetailsPage.CancelThisAndUpcomingDays(CancellationReasons.EquipmentIncompatible);
  
            bookingDetailsPage.ValidateBookingIsCancelled();
            // Return to the booking list and validate the details page for each of the subsequent days in the multi-day hearing
            var hearingDates = ExtractDatesForMultiDayHearing(hearingDto)
                .Where(d => d > hearingDto.ScheduledDateTime)
                .ToList();
            foreach (var date in hearingDates)
            {
                hearingDto.ScheduledDateTime = date;
                bookingDetailsPage = SearchAndViewHearing(bookingDetailsPage, hearingDto);
                bookingDetailsPage.ValidateBookingIsCancelled();
            }
            Assert.Pass();
        }
    }
}
