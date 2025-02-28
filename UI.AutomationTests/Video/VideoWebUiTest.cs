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
        var participant = InitVideoWebParticipant(username, JourneyType.Judge, HearingTestData.ClerkVideoFileName);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsJudge(username, password);

    }

    protected PanelMemberHearingListPage LoginAsPanelMember(string username, string password)
    {
        var panelMember = InitVideoWebParticipant(username, JourneyType.PanelMember, HearingTestData.ClerkVideoFileName);
        var loginPage = NavigateToVideoWeb(panelMember.Driver.GetDriver());
         return loginPage.LoginAsPanelMember(username, password);
    }
    
    protected StaffMemberVenueListPage LoginAsStaffMember(string username, string password)
    {
        var participant = InitVideoWebParticipant(username, JourneyType.StaffMember, HearingTestData.ClerkVideoFileName);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsStaffMember(username, password);
    }

    protected VhoVenueSelectionPage LoginAsVho(string username, string password)
    {
        var participant = InitVideoWebParticipant(username, JourneyType.Vho, null);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return loginPage.LogInAsVho(username, password);
    }

    protected ParticipantHearingListPage LoginAsParticipant(string username, string password, bool isRep, string videoFileName, bool isNew = false)
    {
        var participant = InitVideoWebParticipant(username, isRep ? JourneyType.Representative : JourneyType.Citizen, videoFileName);
        var loginPage = NavigateToVideoWeb(participant.Driver.GetDriver());
        return isNew 
            ? loginPage.LogInAsNewParticipant(username, password)
            : loginPage.LogInAsParticipant(username, password);
    }

    protected QuickLinkJoinYourHearingPage LoginAsQuickLinkUser(string quickLinkJoinUrl, string displayName, string videoFileName = null)
    {
        var participant = InitVideoWebParticipant(displayName, JourneyType.QuickLinkParticipant, videoFileName);
        var driver = participant.Driver.GetDriver();
        driver.Navigate().GoToUrl(quickLinkJoinUrl);
        return new QuickLinkJoinYourHearingPage(driver, EnvConfigSettings.DefaultElementWait);
    }
    
    protected PexipWebAppPage LoginAsJvsEndpoint(string jvsUrl, string displayName){
        var participant = InitVideoWebParticipant(displayName, JourneyType.Jvs, HearingTestData.Individual02FileName);
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

    private VideoWebParticipant InitVideoWebParticipant(string username, JourneyType journeyType, string videoFileName)
    {
        var vhDriver = CreateDriver(username, videoFileName);
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
    /// Sign all users out
    /// </summary>
    protected void SignOutAllUsers()
    {
        foreach (var videoWebParticipant in ParticipantDrivers.Values.Where(x=> x.JourneyType != JourneyType.Jvs))
            SignOut(videoWebParticipant);
    }

    protected static void SignOut(VideoWebParticipant videoWebParticipant)
    {
        TestContext.Out.WriteLine($"Signing out of participant {videoWebParticipant.Username}");
        var confirmSignOut = videoWebParticipant.JourneyType != JourneyType.QuickLinkParticipant;
        // QL participants do not autheneticate with AD so they do not have to sign out like other users
        videoWebParticipant.VhVideoWebPage.SignOut(confirmSignOut);
    }
    
    protected JudgeWaitingRoomPage JudgeLoginToWaitingRoomJourney(BookingDto bookingDto, Guid conferenceId)
    {
        var judge = bookingDto.Judge;
        var username = judge.Username;
        var password = EnvConfigSettings.UserPassword;
        var hearingListPage = LoginAsJudge(username, password);
        var waitingRoomPage = hearingListPage.SelectHearing(conferenceId);
        ParticipantDrivers[judge.Username].VhVideoWebPage = waitingRoomPage;
        return waitingRoomPage;
    }
    
    protected ParticipantWaitingRoomPage ParticipantLoginToWaitingRoomJourney(BookingParticipantDto participant, Guid conferenceId)
    {
        var username = participant.Username;
        var password = EnvConfigSettings.UserPassword;
        var hearingListPage = LoginAsParticipant(username, password, participant.Role == GenericTestRole.Representative, participant.VideoFileName);
        return ParticipantJourneyToWaitingRoom(participant, hearingListPage, username, conferenceId);
    }

    protected ParticipantWaitingRoomPage ParticipantJourneyToWaitingRoom(
        BookingParticipantDto participant, 
        ParticipantHearingListPage participantHearingList, 
        string participantUsername,
        Guid conferenceId)
    {
        var participantWaitingRoomPage = participantHearingList
            .SelectHearing(conferenceId).GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
        // store the participant driver in a dictionary, so we can access it later to sign out
        ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoomPage;
        return participantWaitingRoomPage;
    }
}