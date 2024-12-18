using System.Net;
using UI.AutomationTests.Models;
using UI.PageModels.Pages.PexipInfinityWeb;
using UI.PageModels.Pages.Video;
using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.QuickLink;
using UI.PageModels.Pages.Video.Vho;
using UI.PageModels.Utilities;
using VideoApi.Client;
using VideoApi.Contract.Enums;
using VideoApi.Contract.Requests;
using VideoApi.Contract.Responses;

namespace UI.AutomationTests.Video;

public abstract class VideoWebUiTest : CommonUiTest
{
    public readonly string AdminLoginUsername = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
    protected VideoApiClient VideoApiClient;

    /// <summary>
    ///     This property is used to book a hearing and publish the success to the test reporter
    /// </summary>
    protected IVhDriver AdminWebDriver;

    /// <summary>
    ///     These drivers will store the participants' drivers for a given hearing
    /// </summary>
    protected Dictionary<string, VideoWebParticipant> ParticipantDrivers = new();

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        BookingsApiClient = await VhApiClientFactory.CreateBookingsApiClient();
        VideoApiClient = await VhApiClientFactory.CreateVideoApiClient();
    }

    [SetUp]
    protected virtual void Setup()
    {
        Environment.SetEnvironmentVariable(VhPage.VHTestNameKey, TestContext.CurrentContext.Test.Name);

        AdminWebDriver = CreateDriver(AdminLoginUsername);
        SetupUiTestReport();
    }

    [TearDown]
    public void TearDown()
    {
        CleanUp();
        var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped ||
                     TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;


        AdminWebDriver.PublishTestResult(passed);
        BuildUiReport(AdminWebDriver, AdminLoginUsername);
        AdminWebDriver.Terminate();
        AdminWebDriver = null;
        
        ParticipantDrivers.Values.ToList().ForEach(x =>
        {
            x.Driver.PublishTestResult(passed);
            BuildUiReport(x.Driver, x.Username);
            x.Driver.Terminate();
        });
        ParticipantDrivers.Clear();
        AccessibilityResultCollection.Clear();
    }

    protected virtual async Task<ConferenceDetailsResponse> GetConference(Guid hearingId)
    {
        var pollCount = 0;
        ConferenceDetailsResponse conferenceResponse;
        do {
            conferenceResponse = await PollForConferenceDetails(); 
            pollCount++;
        } while (conferenceResponse == null);

        return conferenceResponse;
        
        async Task<ConferenceDetailsResponse> PollForConferenceDetails()
        {
            try
            {
                var result = await VideoApiClient.GetConferenceDetailsByHearingRefIdsAsync(
                    new GetConferencesByHearingIdsRequest()
                    {
                        IncludeClosed = true,
                        HearingRefIds = [hearingId]
                    });
                result.Should().NotBeNullOrEmpty();
                return result.FirstOrDefault(x => x.CurrentStatus != ConferenceState.Closed) ?? result.First();
            }
            catch (VideoApiException e)
            {
                if (pollCount >= 3)
                    throw new NotFoundException($"Conference not found for hearing {hearingId} after 3 attempts");

                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    return null;
                }

                throw;
            }
        }
    }
    
    protected virtual Task CleanUp()
    {
        return Task.CompletedTask;
    }
    
    protected JudgeHearingListPage LoginAsJudge(string username, string password)
    {
        var participant = InitVideoWebParticipant(username, JourneyType.Judge);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsJudge(username, password);
    }
    
    protected StaffMemberVenueListPage LoginAsStaffMember(string username, string password)
    {
        var participant = InitVideoWebParticipant(username, JourneyType.StaffMember);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsStaffMember(username, password);
    }

    protected VhoVenueSelectionPage LoginAsVho(string username, string password)
    {
        var participant = InitVideoWebParticipant(username, JourneyType.Vho);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsVho(username, password);
    }

    protected ParticipantHearingListPage LoginAsParticipant(string username, string password, bool isRep)
    {
        var participant = InitVideoWebParticipant(username, isRep ? JourneyType.Representative : JourneyType.Citizen);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsParticipant(username, password);
    }

    protected QuickLinkJoinYourHearingPage LoginAsQuickLinkUser(string quickLinkJoinUrl, string displayName)
    {
        var participant = InitVideoWebParticipant(displayName, JourneyType.QuickLinkParticipant);
        var driver = participant.Driver.GetDriver();
        driver.Navigate().GoToUrl(quickLinkJoinUrl);
        return new QuickLinkJoinYourHearingPage(driver, EnvConfigSettings.DefaultElementWait);
    }
    
    protected PexipWebAppPage LoginAsJvsEndpoint(string jvsUrl, string displayName){
        var participant = InitVideoWebParticipant(displayName, JourneyType.Jvs);
        var driver = participant.Driver.GetDriver();
        driver.Navigate().GoToUrl(jvsUrl);
        return new PexipWebAppPage(driver, EnvConfigSettings.DefaultElementWait);
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
        return $"{EnvConfigSettings.VideoUrl}/vh-signin";
    }

    private VideoWebLoginPage NavigateToVideoWeb(IWebDriver driver)
    {
        var url = GetSignInUrl();
        driver.Navigate().GoToUrl(url);
        return new VideoWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
    }

    private VideoWebParticipant InitVideoWebParticipant(string username, JourneyType journeyType)
    {
        var vhDriver = CreateDriver(username);
        var participant = new VideoWebParticipant
        {
            Driver = vhDriver,
            Username = username,
            JourneyType = journeyType
        };
        ParticipantDrivers[username] = participant;
        return participant;
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
        foreach (var videoWebParticipant in ParticipantDrivers.Values.Where(x=> x.JourneyType != JourneyType.Jvs))
        {
            TestContext.Out.WriteLine($"Signing out of participant {videoWebParticipant.Username}");
            videoWebParticipant.VhVideoWebPage.SignOut(videoWebParticipant.JourneyType != JourneyType.QuickLinkParticipant);
        }
    }
}