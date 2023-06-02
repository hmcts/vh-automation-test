using FluentAssertions;

namespace UI.NUnitVersion.Admin.Booking;

public class BookHearingTests : AdminWebUiTest
{
    [Test]
    public void BookAHearing()
    {
        var currentTime = DateTime.Now.ToString("M-d-yy-H-mm-ss");
        var caseName = $"BookAHearing Automation Test {currentTime}";
        var caseNumber = "Automation Test Hearing";
        var caseType = "Civil";
        var hearingType = "Enforcement Hearing";
        var scheduledDateTime = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var durationHour = 1;
        var durationMinute = 30;
        var venueName = "Birmingham Civil and Family Justice Centre";
        var roomName = "Room 1";
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(Username, EnvConfigSettings.UserPassword);

        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        
        createHearingPage.EnterHearingDetails(caseNumber, caseName, caseType, hearingType);
        
        var hearingSchedulePage = createHearingPage.GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(scheduledDateTime, durationHour, durationMinute, venueName,
            roomName);
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails("auto_aw.judge_02@hearings.reform.hmcts.net", "Auto Judge", "");
        
        var addParticipantPage = assignJudgePage.GoToNextPage();
        addParticipantPage.AddExistingIndividualParticipant("Claimant", "Litigant in person", "auto_vw.individual_60@hmcts.net", "Auto 1");
        addParticipantPage.AddExistingRepresentative("Claimant", "Representative", "auto_vw.representative_139@hmcts.net", "Auto 2", "Auto 1");
        addParticipantPage.AddExistingIndividualParticipant("Defendant", "Litigant in person", "auto_vw.individual_137@hmcts.net", "Auto 3");
        addParticipantPage.AddExistingRepresentative("Defendant", "Representative", "auto_vw.representative_157@hmcts.net", "Auto 4", "Auto 3");
        
        var videoAccessPointsPage = addParticipantPage.GoToNextPage();
        
        var otherInformationPage = videoAccessPointsPage.GoToNextPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation("This is a test info");
        
        var summaryPage = otherInformationPage.GoToNextPage();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.ClickViewBookingLink();

        dashboardPage = confirmationPage.GoToDashboardPage();
        
        var postBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var postBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var postBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var postBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();

        postBookingUnallocatedHearingsToday.Should().Be(preBookingUnallocatedHearingsToday);
        postBookingUnallocatedHearingsTomorrow.Should().BeGreaterThan(preBookingUnallocatedHearingsTomorrow);
        postBookingUnallocatedHearingsNextSevenDays.Should().BeGreaterThan(preBookingUnallocatedHearingsNextSevenDays);
        postBookingUnallocatedHearingsNextThirtyDays.Should()
            .BeGreaterThan(preBookingUnallocatedHearingsNextThirtyDays);
        
        Assert.Pass();
    }
}