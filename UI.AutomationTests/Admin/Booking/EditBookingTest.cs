
using UI.Common.Utilities;

namespace UI.AutomationTests.Admin.Booking;

public class EditBookingTest : HearingTest
{
    [Test]
    [Category("admin")]
    public void should_update_booking_schedule_and_change_judge()
    {
        var hearingScheduledDateAndTime = DateUtil
            .GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(60);
        var hearingDto = HearingTestData.CreateHearingDtoWithEndpoints(HearingTestData.JudgePersonalCode,
            judgeUsername: "auto_aw.judge_02@hearings.reform.hmcts.net",
            scheduledDateTime: hearingScheduledDateAndTime);
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);

        var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
        var summaryPage =
            bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);

        //Assign a new Judge 
        var alternativeJudge = new BookingJudiciaryParticipantDto(HearingTestData.AltJudgePersonalCode,
            HearingTestData.AltJudgeUsername,
            "Auto Judge 2", "");

        var assignJudgePage = summaryPage.ChangeJudgeV2();
        assignJudgePage.EnterJudgeDetails(alternativeJudge);
        summaryPage = assignJudgePage.GotToNextPageOnEdit();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        var bookingDetailPage = confirmationPage.ClickViewBookingLink();
        hearingDto.Judge = alternativeJudge;
        bookingDetailPage.ValidateDetailsPage(hearingDto);
        Assert.Pass();
    }

    [Test]
    [FeatureToggleSetting(FeatureToggle.SpecialMeasuresKey, true)]
    [Category("admin")]
    public void EditAHearingWithScreening()
    {
        // TODO
    }
}
