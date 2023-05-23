using UI.NUnitVersion.Drivers;
using UI.PageModels.Pages.Admin;

namespace UI.NUnitVersion;

public class UploadWorkHourTests
{
    private IVhDriver _vhDriver;
    private EnvironmentConfigSettings _envConfigSettings;
    public string username = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    
    [SetUp]
    public void Setup()
    {
        var config = ConfigRootBuilder.Build();
        // _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);
        _envConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings").Get<EnvironmentConfigSettings>();
        _vhDriver = _envConfigSettings.RunOnSaucelabs ? new RemoteChromeVhDriver() : new LocalChromeVhDriver();
    }
    
    [TearDown]
    public void TearDown()
    {
        _vhDriver.PublishTestResult(TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed);
        _vhDriver.Terminate();
    }

    [Test]
    public void UploadValidWorkHours()
    {
        var driver = _vhDriver.GetDriver();
        driver.Navigate().GoToUrl(_envConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, _envConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(username, _envConfigSettings.UserPassword);
        
        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();
        workAllocationPage.UploadWorkHoursFile(Path.Join("TestData", "GoodWorkHours.csv"));
        workAllocationPage.WaitForFileUploadSuccessMessage();
        Assert.Pass();
    }
    
    [Test]
    public void UploadValidNonAvailabilityHours()
    {
        var driver = _vhDriver.GetDriver();
        driver.Navigate().GoToUrl(_envConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, _envConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(username, _envConfigSettings.UserPassword);
        
        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();
        workAllocationPage.UploadNonWorkHoursFile(Path.Join("TestData", "GoodNonAvailabilityHours.csv"));
        workAllocationPage.WaitForFileUploadSuccessMessage();
        Assert.Pass();
    }
}