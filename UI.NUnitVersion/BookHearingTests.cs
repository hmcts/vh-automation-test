using UI.NUnitVersion.Drivers;

namespace UI.NUnitVersion;

public class BookHearingTests
{
    private IVhDriver _vhDriver;
    // private IWebDriver _driver;
    private EnvironmentConfigSettings _envConfigSettings;
    public string username = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    // private TestReporter _testReporter;

    [OneTimeSetUp]
    protected void OneTimeSetup()
    {
        // _testReporter = new TestReporter();
        // _testReporter.SetupReport();
    }
    
    [SetUp]
    public void Setup()
    {
        var config = ConfigRootBuilder.Build();
        // _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);
        _envConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings").Get<EnvironmentConfigSettings>();
        // _driver = new LocalChromeVhDriver().GetDriver(); //BuildChromeDriver();
        _vhDriver = new RemoteChromeVhDriver();
    }
    
    [TearDown]
    public void TearDown()
    {
        // _testReporter.ProcessTest();
        _vhDriver.PublishTestResult(TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed);
        _vhDriver.Terminate();
    }
    
    [OneTimeTearDown]
    protected void OneTimeTearDown()
    {
        // _testReporter.Flush();
    }

    [Test]
    public void BookAHearing()
    {
        var driver = _vhDriver.GetDriver();
        driver.Navigate().GoToUrl(_envConfigSettings.AdminUrl);
        var loginPage = new LoginPage(driver, _envConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(username, _envConfigSettings.UserPassword);
        
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