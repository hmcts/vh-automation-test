using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using UI.NUnitVersion.Configuration;
using UI.NUnitVersion.Reporters;
using UI.PageModels.Pages;
using WebDriverManager.DriverConfigs.Impl;

namespace UI.NUnitVersion;

public class BookHearingTests
{
    private IWebDriver _driver;
    private EnvironmentConfigSettings _envConfigSettings;
    public string username = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    private TestReporter _testReporter;

    [OneTimeSetUp]
    protected void OneTimeSetup()
    {
        _testReporter = new TestReporter();
        _testReporter.SetupReport();
    }
    
    [SetUp]
    public void Setup()
    {
        var config = ConfigRootBuilder.Build();
        _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);
        _envConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings").Get<EnvironmentConfigSettings>();
        _driver = BuildChromeDriver();
        
    }
    
    [TearDown]
    public void TearDown()
    {
        _testReporter.ProcessTest();
        _driver.Quit();
    }
    
    [OneTimeTearDown]
    protected void OneTimeTearDown()
    {
        _testReporter.Flush();
    }

    [Test]
    public void BookAHearing()
    {
        _driver.Navigate().GoToUrl(_envConfigSettings.AdminUrl);
        var loginPage = new LoginPage(_driver, _envConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(username, _envConfigSettings.UserPassword);
        _testReporter.CaptureScreenshot(_driver, "Dashboard", "Post Login");
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails("Test Hearing", "Test Case", "Civil", "Enforcement Hearing");
        _testReporter.CaptureScreenshot(_driver, "Hearing details", "Post entering hearing details");
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        hearingSchedulePage.EnterSingleDayHearingSchedule(DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30), 1, 30,
            "Birmingham Civil and Family Justice Centre", "Room 1");
        _testReporter.CaptureScreenshot(_driver, "Hearing details", "Choosing venue and room");
        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails("Manual01Clerk01@hearings.reform.hmcts.net", "Judge Fudge", "");
        _testReporter.CaptureScreenshot(_driver);
        Assert.Pass();
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
    
    private IWebDriver BuildChromeDriver()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        var cService = ChromeDriverService.CreateDefaultService();
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("start-maximized");
        chromeOptions.AddArgument("no-sandbox");
        chromeOptions.AddArguments("--use-fake-ui-for-media-stream");
        chromeOptions.AddArguments("--use-fake-device-for-media-stream");
        var webDriver = new ChromeDriver(cService, chromeOptions);
        return webDriver;
    }
}