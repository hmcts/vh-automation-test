using NUnit.Framework.Interfaces;
using UI.NUnitVersion.Models;
using UI.PageModels.Pages.Video;
using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.Vho;

namespace UI.NUnitVersion.Video;

public abstract class VideoWebUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";

    /// <summary>
    ///     This property is used to book a hearing and publish the success to the test reporter
    /// </summary>
    protected IVhDriver AdminWebDriver;

    protected EnvironmentConfigSettings EnvConfigSettings;

    /// <summary>
    ///     These drivers will store the participants' drivers for a given hearing
    /// </summary>
    protected Dictionary<string, VideoWebParticipant> ParticipantDrivers = new();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var config = ConfigRootBuilder.Build();
        EnvConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings")
            .Get<EnvironmentConfigSettings>();
    }

    [SetUp]
    protected virtual void Setup()
    {
        AdminWebDriver = CreateDriver("AdminWeb");
        // _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);   
    }

    [TearDown]
    public void TearDown()
    {
        var testResult = TestContext.CurrentContext.Result.Outcome.Status ==
                         TestStatus.Passed;
        AdminWebDriver.PublishTestResult(testResult);
        AdminWebDriver.Terminate();
        AdminWebDriver = null;
        ParticipantDrivers.Values.ToList().ForEach(x =>
        {
            x.Driver.PublishTestResult(testResult);
            x.Driver.Terminate();
        });
        ParticipantDrivers.Clear();
    }

    protected JudgeHearingListPage LoginAsJudge(string username, string password)
    {
        var participant = InitVideoWebParticipant(username);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsJudge(username, password);
    }

    protected VhoVenueSelectionPage LoginAsVho(string username, string password)
    {
        var participant = InitVideoWebParticipant(username);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsVho(username, password);
    }

    protected StaffMemberHearingListPage LoginAsStaffMember(string username, string password)
    {
        var participant = InitVideoWebParticipant(username);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsStaffMember(username, password);
    }

    protected ParticipantHearingListPage LoginAsParticipant(string username, string password)
    {
        var participant = InitVideoWebParticipant(username);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsParticipant(username, password);
    }

    /// <summary>
    /// To avoid getting caught out by the IDP selection page when the toggle is turned on, retrieve the IDP specific sign-in url.
    /// </summary>
    /// <returns></returns>
    private string GetSignInUrl()
    {
        // https://video.hearings.reform.hmcts.net/vh-signin
        // https://video.hearings.reform.hmcts.net/ejud-signin
        // https://video.hearings.reform.hmcts.net/justice-signin
        return $"{EnvConfigSettings.VideoUrl}vh-signin";
    }

    private VideoWebLoginPage NavigateToVideoWeb(IWebDriver driver)
    {
        var url = GetSignInUrl();
        driver.Navigate().GoToUrl(url);
        return new VideoWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
    }

    private VideoWebParticipant InitVideoWebParticipant(string username)
    {
        var vhDriver = CreateDriver(username);
        var participant = new VideoWebParticipant
        {
            Driver = vhDriver,
            Username = username,
            JourneyType = JourneyType.Judge
        };
        ParticipantDrivers[username] = participant;
        return participant;
    }

    private IVhDriver CreateDriver(string username = null)
    {
        return EnvConfigSettings.RunOnSaucelabs
            ? new RemoteChromeVhDriver(username: username)
            : new LocalChromeVhDriver();
    }

    /// <summary>
    /// Sign out of a participant's session
    /// </summary>
    /// <param name="username"></param>
    protected void SignOutAs(string username)
    {
        ParticipantDrivers[username].VhVideoWebPage.SignOut();
    }

    /// <summary>
    /// Sign all users out
    /// </summary>
    protected void SignOutAllUsers()
    {
        foreach (var videoWebParticipant in ParticipantDrivers.Values)
        {
            TestContext.WriteLine($"Signing out of participant {videoWebParticipant.Username}");
            videoWebParticipant.VhVideoWebPage.SignOut();
        }
    }
}