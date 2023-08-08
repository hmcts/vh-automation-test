using BookingsApi.Client;
using BookingsApi.Contract.Requests.Enums;

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

    /// <summary>
    /// Run ad-hoc clean up tasks for a test
    /// </summary>
    /// <returns></returns>
    protected virtual Task CleanUp()
    {
        return Task.CompletedTask;
    }

    protected async Task<JusticeUserResponse> CreateVhTeamLeaderJusticeUserIfNotExist(string username)
    {
        var matchedUsers = await BookingsApiClient.GetJusticeUserListAsync(username, true);
        var justiceUser = matchedUsers.FirstOrDefault(x =>
            x.ContactEmail.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        if (justiceUser == null)
        {
            justiceUser = await BookingsApiClient.AddJusticeUserAsync(new AddJusticeUserRequest()
            {
                Username = username,
                ContactEmail = username,
                ContactTelephone = null,
                FirstName = "Auto",
                LastName = "VHoteamleader",
                Roles = new List<JusticeUserRole>()
                {
                    JusticeUserRole.VhTeamLead
                },
                CreatedBy = "automation test framework"
            });
            TestContext.WriteLine($"Created user {justiceUser.ContactEmail}");
        }

        if (justiceUser.Deleted)
        {
            TestContext.WriteLine("Restoring deleted user {justiceUser.ContactEmail}");
            await BookingsApiClient.RestoreJusticeUserAsync(new RestoreJusticeUserRequest()
            {
                Id = justiceUser.Id, Username = justiceUser.Username
            });
        }

        if (!justiceUser.IsVhTeamLeader)
        {
            TestContext.WriteLine("Updated justice user to be a Team Leader");
            await BookingsApiClient.EditJusticeUserAsync(new EditJusticeUserRequest()
            {
                Id = justiceUser.Id, Username = justiceUser.Username,
                Roles = new List<JusticeUserRole>() {JusticeUserRole.VhTeamLead}
            });
        }

        TestContext.WriteLine($"Using justice user for test {justiceUser.ContactEmail}");

        return justiceUser;
    }
}