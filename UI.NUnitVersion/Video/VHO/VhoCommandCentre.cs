using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.Vho;
using VideoApi.Contract.Responses;
using static System.Threading.Thread;

namespace UI.NUnitVersion.Video.VHO;

public class VhoDashboard : VideoWebUiTest
{

    private HearingDetailsResponse _hearing;
    private ConferenceDetailsResponse _conference;
    private BookingExistingParticipantDto _testParticipant;
    private const int RetrySleep = 3000;
    private const int RetryLimit = 6;

    [SetUp]
    public new async Task Setup()
    {
        var date = DateTime.Now.AddMinutes(1);
        _testParticipant = HearingTestData.KnownParticipantsForTesting().First();
        var request = HearingTestData.CreateNewRequestDtoWithKnownParticipants(scheduledDateTime: date, existingParticipantDtos: _testParticipant);
        _hearing = await BookingsApiClient.BookNewHearingAsync(request);
        for (var retry = 0; retry <= RetryLimit; retry++)
        {
            try
            {
                _conference = await VideoApiClient.GetConferenceByHearingRefIdAsync(_hearing.Id, false);
                break;
            }
            catch (Exception)
            {
                Sleep(RetrySleep);
            }
            if (retry == RetryLimit)
                throw new TimeoutException($"Unable to find conference for hearing id {_hearing.Id} after {RetryLimit} attempts");
        }
    }

    [Test]
    public void CommandCentreEndToEndJourney()
    {
        //Login
        var vhoVenueSelectionPage = LoginAsVho(HearingTestData.VhOfficerUsername, EnvConfigSettings.UserPassword);
 
        // CSO will be able to see hearings at selected venue(s)
        // OR selected CSO(s) TODO: Need to add this functionality in setup and allocated the created hearing to a CSO
        var commandCentrePage = vhoVenueSelectionPage.SelectHearingsByVenues(new List<string> { _hearing.HearingVenueName});
       
        // Hearings will be displayed in chronological order as per VIH-10224
        ValidateHearingsAreInChronologicalOrder(commandCentrePage);
        
        // CSO will be able to select a hearing to monitor
        commandCentrePage.SelectConferenceFromList(_conference.Id.ToString());
        
        // CSO will be able to see connectivity status of participants in that hearing
        var testParticipantVideoWebPage = ParticipantStatusScenario(commandCentrePage);

        // CSO will be able to see participants moving from waiting room to consultation room and hearing room
        InstantMessagingScenario(commandCentrePage, testParticipantVideoWebPage);
                
        // CSO will be able to see the Kinly iFrame
        TestingKinlyiframeScenario();
        
        // CSO will be able to monitor HearingStatus updates
        HearingStatusScenario(commandCentrePage);
        
        // CSO will return to the select list/CSO's page; current selection will be displayed 
        commandCentrePage.ChangeVenueSelection();        
        
        Assert.Pass();
    }

    private void ValidateHearingsAreInChronologicalOrder(CommandCentrePage commandCentrePage)
    {
        var conferences = commandCentrePage.GetAllConferencesStartTimes();
        var hearingStartTimes = conferences
            .Select(x => DateTime.ParseExact(x.Text, "HH:mm", CultureInfo.InvariantCulture))
            .ToList();
        var orderedStartTimes = hearingStartTimes.OrderBy(x => x).ToList();
        for (var i = 0; i < hearingStartTimes.Count; i++)
            hearingStartTimes[i].Should().Be(orderedStartTimes[i]);
    }

    private VhVideoWebPage ParticipantStatusScenario(CommandCentrePage commandCentrePage)
    {
        var hearingPanel = commandCentrePage.ClickHearingsButton();
        var testParticipantId = _conference.Participants.First(x => x.Username == _testParticipant.Username).Id;
        hearingPanel.ParticipantStatus(testParticipantId).Should().Be("Not signed in");
        var testParticipantVideoWebPage = LoginWithTestParticipant();
        var participantStatus = hearingPanel.ParticipantStatus(testParticipantId);
        //Can be slow to propagate the status change
        if (participantStatus == "Not signed in")
            commandCentrePage.ReloadPage()
                .SelectConferenceFromList(_conference.Id.ToString())
                .ClickHearingsButton();
        hearingPanel.ParticipantStatus(testParticipantId).Should().BeOneOf("Available", "Joining");
        return testParticipantVideoWebPage;
    }

    private void HearingStatusScenario(CommandCentrePage commandCentrePage)
    {
        var hearingPanel = commandCentrePage.ClickHearingsButton();
        hearingPanel.HearingStatus().Should().Be("Not Started");
        var judgeVideoWebPage = LogIntoHearingWithJudge();
        
        JudgeHearingRoomPage judgeWaitingRoom;
        try
        {
            //Give time to initialise the waiting room so that 
            Sleep(10000);
            judgeWaitingRoom = judgeVideoWebPage.StartOrResumeHearing();
        }
        catch (WebDriverTimeoutException ex)
        {
            Assert.Fail("Judge could not start the hearing after 30 seconds, due to start button being disabled. " +
                        "Unable to continue with test scenario.");
            throw ex;
        }
        
        if(hearingPanel.HearingStatus() == "Not Started")
            commandCentrePage.ReloadPage()
                .SelectConferenceFromList(_conference.Id.ToString())
                .ClickHearingsButton();
        hearingPanel.HearingStatus().Should().Be("In Session");
        
        judgeWaitingRoom.PauseHearing();
        if(hearingPanel.HearingStatus() == "In Session")
            commandCentrePage.ReloadPage()
                .SelectConferenceFromList(_conference.Id.ToString())
                .ClickHearingsButton();
        hearingPanel.HearingStatus().Should().Be("Paused");
    }

    //TODO: Will be implemented with VIH-10277
    private void TestingKinlyiframeScenario()
    {
        // CSO will be able to start a consultation call with a participant
        // CSO will be able to change a participants display name
    }

    private void InstantMessagingScenario(CommandCentrePage commandCentrePage, VhVideoWebPage testParticipantVideoWebPage)
    {
        // CSO will be able to IM a participant
        var messagingPanel = commandCentrePage.ClickMessagesButton();
        messagingPanel.SelectUserFromMessagesList(_testParticipant.DisplayName);
        var messageFromTheCso = "Hello from the CSO";
        messagingPanel.SendAMessage(messageFromTheCso);
        var messagesSent = messagingPanel.GetMessagesSent();
        messagesSent.Last().Text.Should().Contain(messageFromTheCso);

        // CSO will be able to receive a reply to the IM
        //TODO: Currently not working IRL due to VPN issue, add assertion back in once that's working
        /*var messageToCso = "Hello from the participant";
        testParticipantVideoWebPage.OpenChatWithVHO();
        testParticipantVideoWebPage.SendAMessageToVHO(messageToCso);
        var messagesReceived = messagingPanel.GetMessagesReceived();
        messagesReceived.Last().Text.Should().Contain(messageToCso);*/
    }

    private VhVideoWebPage LoginWithTestParticipant()
    {
        var participantHearingList =
            LoginAsParticipant(_testParticipant.Username, EnvConfigSettings.UserPassword, false);
        var waitingRoom = participantHearingList.SelectHearing(_hearing.Cases[0].Name)
            .GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage().SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes()
            .SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration();
        return waitingRoom;
    }

    private JudgeWaitingRoomPage LogIntoHearingWithJudge()
    {
        var judgeUsername = _hearing.Participants.Find(e => e.HearingRoleName == "Judge").Username;
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        return judgeHearingListPage.SelectHearing(_hearing.Cases[0].Name);
    }

    [TearDown]
    protected override async Task CleanUp()
    {
        if(_hearing != null)
        {
            await BookingsApiClient.RemoveHearingAsync(_hearing.Id);
            TestContext.WriteLine($"Removed Hearing {_hearing.Id}");
            _hearing = null;
        }
    }
}