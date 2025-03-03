
using UI.Common.Utilities;
using UI.PageModels.Pages.Admin.Booking;

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
    public async Task EditAHearingWithScreening()
    {
        // Book a hearing with screening participants
        var hearingScheduledDateAndTime = DateUtil
            .GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(60);
        var hearingDto = HearingTestData.CreateHearingDto(HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, scheduledDateTime: hearingScheduledDateAndTime);
        var participant1 = hearingDto.Participants[0];
        var participant2 = hearingDto.Participants[1];
        participant1.Screening = new ScreeningDto
        {
            ProtectedFrom = [new ScreeningEntityDto(participant2.DisplayName)] 
        };
        await BookHearing(hearingDto);
        
        // Log in and go to the edit special measures page
        var bookingDetailsPage = LogInAndJourneyToBookingDetailsPage(hearingDto.CaseNumber);
        var specialMeasuresPage = bookingDetailsPage.EditSpecialMeasures();
        
        // Remove screening and add a different one
        specialMeasuresPage.RemoveScreening(0);
        participant1.Screening = null;
        participant2.Screening = new ScreeningDto
        {
            ProtectedFrom = [new ScreeningEntityDto(participant1.DisplayName)] 
        };
        specialMeasuresPage.AddScreeningParticipants(hearingDto.ScreeningParticipants);

        // Save and verify the updated booking
        var summaryPage = specialMeasuresPage.GoToSummaryPage();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        var bookingDetailPage = confirmationPage.ClickViewBookingLink();
        bookingDetailPage.ValidateScreeningDetails(hearingDto);
    }
    
    private async Task BookHearing(BookingDto bookingDto)
    {
        var request = HearingTestData.CreateRequest(bookingDto);
        var hearing = await BookingsApiClient.BookNewHearingWithCodeAsync(request);
        TestHearingIds.Add(hearing.Id.ToString());
    }
}
