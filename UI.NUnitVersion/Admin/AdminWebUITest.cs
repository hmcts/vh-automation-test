using BookingsApi.Client;

namespace UI.NUnitVersion.Admin;

public abstract class AdminWebUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected EnvironmentConfigSettings EnvConfigSettings;
    protected BookingsApiClient BookingsApiClient;

    protected IVhDriver VhDriver;
    // protected TestReporter _testReporter;

    [OneTimeSetUp]
    protected virtual async Task OneTimeSetup()
    {
        var config = ConfigRootBuilder.Build();
        EnvConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings")
            .Get<EnvironmentConfigSettings>();
        BookingsApiClient = await VhApiClientFactory.CreateBookingsApiClient();
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

    protected virtual Task CleanUp()
    {
        return Task.CompletedTask;
    }
}