using UI.PageModels.Pages.Admin.Booking;

namespace UI.NUnitVersion.Admin.Booking;

[TestFixture]
public class BookingHearingWithEndPointTest : AdminWebUiTest
{
    [Test]
    public void BookAHearingEndPoint()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var bookingDto = HearingTestData.CreateHearingEndPoint(scheduledDateTime: date);
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();

        createHearingPage.EnterHearingDetails(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType,
            bookingDto.HearingType);

        var hearingSchedulePage = createHearingPage.GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(bookingDto.ScheduledDateTime, bookingDto.DurationHour,
            bookingDto.DurationMinute, bookingDto.VenueName, bookingDto.RoomName);
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails("auto_aw.judge_02@hearings.reform.hmcts.net", "Auto Judge", "");

        var addParticipantPage = assignJudgePage.GoToParticipantsPage();
        addParticipantPage.AddExistingParticipants(bookingDto.Participants);
        addParticipantPage.GoToVideoAccessPointsPage();
        addParticipantPage.CompleteBookAVideoHearing("EndPoint 4", "EndPoint 4");
        
        var summaryPage = addParticipantPage.GoToSummaryPage();

        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.ClickViewBookingLink();
    }
}