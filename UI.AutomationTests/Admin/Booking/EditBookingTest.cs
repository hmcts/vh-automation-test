using UI.Common.Utilities;

namespace UI.AutomationTests.Admin.Booking;


public class EditBookingTest : HearingTest
{
    [Test]
    [Category("admin")]
    public void should_update_booking_schedule_and_change_judge()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(60);
        var hearingDto = HearingTestData.CreateHearingDtoWithEndpoints(HearingTestData.JudgePersonalCode,
            judgeUsername: "auto_aw.judge_02@hearings.reform.hmcts.net",
            scheduledDateTime: hearingScheduledDateAndTime);
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);
        
        var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
        var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
        
        //Assign a new Judge 
        var alternativeJudge = new BookingJudgeDto(HearingTestData.AltJudgePersonalCode, HearingTestData.AltJudgeUsername,
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
    [Category("admin")]
    [FeatureToggleSetting(FeatureToggle.InterpreterEnhancementsToggleKey, true)]
    public void should_update_booking_with_interpreter_languages()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var interpreterLanguage = new InterpreterLanguageDto("Spanish", InterpreterType.Verbal);
        var bookingDto = HearingTestData.CreateHearingDtoWithInterpreterLanguages(HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, scheduledDateTime: date, interpreterLanguage);
        bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(bookingDto);
        
        var summaryPage = bookingDetailsPage.UpdateSchedule(bookingDto.ScheduledDateTime, bookingDto.DurationHour, bookingDto.DurationMinute);

        var newInterpreterLanguage = new InterpreterLanguageDto("British Sign Language (BSL)", InterpreterType.Sign);
        
        // Assign a new judge
        var alternativeJudge = new BookingJudgeDto(HearingTestData.JudgePersonalCode, HearingTestData.AltJudgeUsername,
            "Auto Judge 2", "")
        {
            InterpreterLanguage = newInterpreterLanguage
        };
        var assignJudgePage = summaryPage.ChangeJudgeV2();
        assignJudgePage.EnterJudgeDetails(alternativeJudge);
        bookingDto.Judge = alternativeJudge;
        summaryPage = assignJudgePage.GotToNextPageOnEdit();

        // Update the participants
        var participantsPage = summaryPage.ChangeParticipants();
        foreach (var participant in bookingDto.Participants.Where(p => p.Role != GenericTestRole.Representative).ToList()) // There is a bug updating representatives, so skip them for now
        {
            participant.InterpreterLanguage = newInterpreterLanguage;
            Thread.Sleep(5000); // Allow time for the edit link to be clickable
            participantsPage.UpdateParticipant(participant.FullName, participant.DisplayName, newInterpreterLanguage);
        }
        var videoAccessPointsPage = participantsPage.GoToVideoAccessPointsPage();
        
        // Update the endpoints
        var sortedEndpoints = bookingDto.VideoAccessPoints.OrderBy(x => x.DisplayName).ToList();
        foreach (var endpoint in sortedEndpoints)
        {
            endpoint.InterpreterLanguage = newInterpreterLanguage;
        }
        const int endpointIndexToUpdate = 0;
        var endpointToUpdate = sortedEndpoints[endpointIndexToUpdate];
        videoAccessPointsPage.UpdateVideoAccessPoint(endpointIndexToUpdate, "None", newInterpreterLanguage);
        endpointToUpdate.DefenceAdvocateDisplayName = "";
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        
        summaryPage = otherInformationPage.GoToSummaryPage();
        
        summaryPage.ValidateSummaryPage(bookingDto);
        var confirmationPage = summaryPage.ClickBookButton();
            
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        var bookingDetailPage = confirmationPage.ClickViewBookingLink();
        bookingDetailPage.ValidateDetailsPage(bookingDto);

        Assert.Pass();
    }
}