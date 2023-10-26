using BookingsApi.Client;
using BookingsApi.Contract.V1.Requests.Enums;
using UI.NUnitVersion.Configuration;

namespace UI.NUnitVersion.Admin;

public abstract class AdminWebUiTest : CommonUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected EnvironmentConfigSettings EnvConfigSettings;
    protected TestDataConfiguration TestDataConfig;

    protected IVhDriver VhDriver;
    // protected TestReporter _testReporter;

    [OneTimeSetUp]
    protected virtual async Task OneTimeSetup()
    {
        EnvConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        BookingsApiClient = await VhApiClientFactory.CreateBookingsApiClient();
        TestDataConfig = ConfigRootBuilder.TestDataConfigurationInstance();
        // _testReporter = new TestReporter();
        // _testReporter.SetupReport();
    }

    [SetUp]
    protected virtual void Setup()
    {
        Environment.SetEnvironmentVariable(VhPage.VHTestNameKey, TestContext.CurrentContext.Test.Name);
        VhDriver = EnvConfigSettings.RunOnSaucelabs ? new RemoteChromeVhDriver() : new LocalChromeVhDriver();
        // _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);
    }

    [TearDown]
    protected virtual void TearDown()
    {
        // _testReporter.ProcessTest();
        CleanUp();
        VhDriver.PublishTestResult(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed);
        VhDriver.Terminate();
    }

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        // _testReporter.Flush();
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