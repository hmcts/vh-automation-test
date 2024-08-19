using UI.PageModels.Constants;

namespace UI.AutomationTests.Admin.Booking
{
    public class CancelHearingTests : HearingTest
    {
        [Category("Daily")]
        [Test]
        public void CancelHearing()
        {
            var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(60);
            var hearingDto = HearingTestData.CreateHearingDtoWithEndpoints(HearingTestData.JudgePersonalCode,
                judgeUsername: "auto_aw.judge_02@hearings.reform.hmcts.net",
                scheduledDateTime: hearingScheduledDateAndTime);
            var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);
            
            bookingDetailsPage.ClickCancelBooking();
            bookingDetailsPage.CancelSingleHearing(CancellationReasons.EquipmentIncompatible);
  
            bookingDetailsPage.ValidateBookingIsCancelled();
            Assert.Pass();
        }
    }
}
