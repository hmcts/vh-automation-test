namespace UI.NUnitVersion.Admin;

public class BookHearingTests : AdminWebUiTest
{
    [Test]
    public void BookAHearing()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(Username, EnvConfigSettings.UserPassword);
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails("Test Hearing", "Test Case", "Civil", "Enforcement Hearing");
        
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        hearingSchedulePage.EnterSingleDayHearingSchedule(DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30), 1, 30,
            "Birmingham Civil and Family Justice Centre", "Room 1");
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails("Manual01Clerk01@hearings.reform.hmcts.net", "Judge Fudge", "");
        
        var addParticipantPage = assignJudgePage.GoToNextPage();
        addParticipantPage.AddExistingIndividualParticipant("Claimant", "Litigant in person", "auto_vw.individual_60@hmcts.net", "Auto 1");
        addParticipantPage.AddExistingRepresentative("Claimant", "Representative", "auto_vw.representative_139@hmcts.net", "Auto 2", "Auto 1");
        // addParticipantPage.AddExistingParticipant("Defendant", "Litigant in person", "auto_vw.individual_137@hmcts.net");
        // addParticipantPage.AddExistingRepresentative("Defendant", "Representative", "auto_vw.representative_157@hmcts.net");
        
        var videoAccessPointsPage = addParticipantPage.GoToNextPage();
        
        var otherInformationPage = videoAccessPointsPage.GoToNextPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation("SP test");
        
        var summaryPage = otherInformationPage.GoToNextPage();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.ClickViewBookingLink();
        Assert.Pass();
    }
}