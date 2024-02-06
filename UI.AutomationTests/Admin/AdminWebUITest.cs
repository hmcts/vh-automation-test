using UI.AutomationTests.Configuration;
using UI.AutomationTests.Drivers;

namespace UI.AutomationTests.Admin;

public abstract class AdminWebUiTest : CommonUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected EnvironmentConfigSettings EnvConfigSettings;
    protected TestDataConfiguration TestDataConfig;
    protected IVhDriver VhDriver;

    [OneTimeSetUp]
    protected virtual async Task OneTimeSetup()
    {
        EnvConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        BookingsApiClient = await VhApiClientFactory.CreateBookingsApiClient();
        TestDataConfig = ConfigRootBuilder.TestDataConfigurationInstance();
    }

    [SetUp]
    protected virtual void Setup()
    {
        Environment.SetEnvironmentVariable(VhPage.VHTestNameKey, TestContext.CurrentContext.Test.Name);
        VhDriver = EnvConfigSettings.RunOnSaucelabs ? new RemoteChromeVhDriver() : new LocalChromeVhDriver();
    }

    [TearDown]
    protected virtual void TearDown()
    {
        CleanUp();
        VhDriver.PublishTestResult(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed);
        VhDriver.Terminate();
    }
    
    /// <summary>
    /// Run ad-hoc clean up tasks for a test
    /// </summary>
    /// <returns></returns>
    protected virtual Task CleanUp()
    {
        return Task.CompletedTask;
    }
}