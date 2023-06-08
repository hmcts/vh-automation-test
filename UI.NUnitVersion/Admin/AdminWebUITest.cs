namespace UI.NUnitVersion.Admin;

public abstract class AdminWebUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected EnvironmentConfigSettings EnvConfigSettings;

    protected IVhDriver VhDriver;
    // protected TestReporter _testReporter;

    [OneTimeSetUp]
    protected virtual void OneTimeSetup()
    {
        var config = ConfigRootBuilder.Build();
        EnvConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings")
            .Get<EnvironmentConfigSettings>();
        // _testReporter = new TestReporter();
        // _testReporter.SetupReport();
    }

    [SetUp]
    protected virtual void Setup()
    {
        VhDriver = EnvConfigSettings.RunOnSaucelabs ? new RemoteChromeVhDriver() : new LocalChromeVhDriver();
        // _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);
    }

    [TearDown]
    protected virtual void TearDown()
    {
        // _testReporter.ProcessTest();
        VhDriver.PublishTestResult(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed);
        VhDriver.Terminate();
    }

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        // _testReporter.Flush();
    }
}