namespace UI.AutomationTests.Admin.Booking;

[Category("Daily")]
public class EditBookingTest : HearingTest
{
    [Test]
    public void should_update_booking_schedule_and_change_judge()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(60);
        var hearingDto = HearingTestData.CreateHearingDtoWithEndpoints(judgeUsername:"auto_aw.judge_02@hearings.reform.hmcts.net",scheduledDateTime:hearingScheduledDateAndTime);
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);
        
        var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
        var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
        
        //Assign a new Judge 
        var alternativeJudge = new BookingJudgeDto(HearingTestData.AltJudge, "Auto Judge 2", "");
        var useV2Api = FeatureToggle.Instance().UseV2Api();
        var assignJudgePage = useV2Api 
            ? summaryPage.ChangeJudgeV2() 
            : summaryPage.ChangeJudgeV1();
        assignJudgePage.EnterJudgeDetails(alternativeJudge, useV2Api);
        summaryPage = assignJudgePage.GotToNextPageOnEdit();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        var bookingDetailPage = confirmationPage.ClickViewBookingLink();
        hearingDto.Judge = alternativeJudge;
        bookingDetailPage.ValidateDetailsPage(hearingDto);
        Assert.Pass();
    }
}