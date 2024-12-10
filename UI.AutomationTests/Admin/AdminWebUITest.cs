using UI.AutomationTests.Reporters;

namespace UI.AutomationTests.Admin;

public abstract class AdminWebUiTest : CommonUiTest
{
    protected readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected IVhDriver VhDriver;

    [OneTimeSetUp]
    protected virtual async Task OneTimeSetup()
    {
        EnvConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        BookingsApiClient = await VhApiClientFactory.CreateBookingsApiClient();
        UserApiClient = await VhApiClientFactory.CreateUserApiClient();
    }

    [SetUp]
    protected virtual async Task Setup()
    {
        Environment.SetEnvironmentVariable(VhPage.VHTestNameKey, TestContext.CurrentContext.Test.Name);
        VhDriver = CreateDriver(AdminLoginUsername);
        
        await InitTest();
        
        SetupUiTestReport();
    }

    /// <summary>
    /// Run ad-hoc clean up tasks for a test
    /// </summary>
    /// <returns></returns>
    private async Task InitTest()
    {
        await CreateVhTeamLeaderJusticeUserIfNotExist(AdminLoginUsername);
    }

    [TearDown]
    protected virtual void TearDown()
    {
        CleanUp();
        if(VhDriver == null) throw new InvalidOperationException("Driver is null, cannot publish test result");
        var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped ||
                      TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
        
        BuildUiReport(VhDriver, AdminLoginUsername);

        VhDriver.PublishTestResult(passed);
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