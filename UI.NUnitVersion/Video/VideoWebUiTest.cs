using UI.NUnitVersion.Models;
using UI.PageModels.Pages.Video;
using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.Vho;

namespace UI.NUnitVersion.Video;

public abstract class VideoWebUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected EnvironmentConfigSettings EnvConfigSettings;
    
    /// <summary>
    /// This property is used to book a hearing and publish the success to the test reporter
    /// </summary>
    protected IVhDriver AdminWebDriver;
    
    /// <summary>
    /// These drivers will store the participants' drivers for a given hearing
    /// </summary>
    protected Dictionary<string, VideoWebParticipant> ParticipantDrivers = new();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var config = ConfigRootBuilder.Build();
        EnvConfigSettings = config.GetSection("SystemConfiguration:EnvironmentConfigSettings").Get<EnvironmentConfigSettings>();
    }
    
    [SetUp]
    protected virtual void  Setup()
    {
        AdminWebDriver = CreateDriver();
        // _testReporter.SetupTest(TestContext.CurrentContext.Test.Name);   
    }

    [TearDown]
    public void TearDown()
    {
        AdminWebDriver.PublishTestResult(TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed);
        AdminWebDriver.Terminate();
        ParticipantDrivers.Values.ToList().ForEach(x => x.Driver.Terminate());
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
    
    private string GetSignInUrl()
    {
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
        var vhDriver = CreateDriver();
        var participant = new VideoWebParticipant()
        {
            Driver = vhDriver,
            Username = username,
            JourneyType = JourneyType.Judge
        };
        ParticipantDrivers[username] = participant;
        return participant;
    }

    private IVhDriver CreateDriver()
    {
        return EnvConfigSettings.RunOnSaucelabs ? new RemoteChromeVhDriver() : new LocalChromeVhDriver();
    }
    
    protected void SignOutAs(string username)
    {
        throw new NotImplementedException();
    }
    
    protected void SignOutAllUsers()
    {
        // loop through all users in VhDrivers and sign out
        throw new NotImplementedException();
    }
}