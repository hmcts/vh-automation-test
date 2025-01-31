using UI.PageModels.Pages.PexipInfinityWeb;
using UI.PageModels.Pages.Video.Participant;

namespace UI.AutomationTests.Video;

public class DailyProdCheck : VideoWebUiTest
{
    private string _hearingIdString;
    private (string SipUrl, string Pin) _sipUrlAndPin;

    [Test]
    [Category("smoketest")]
    [Description("Daily smoke test with a judge, individual and jvs endpoint")]
    public async Task DailySmokeTest()
    {
        var hearingDto = CreateABookingForSmokeTest();

        await BookHearing(hearingDto);

        // log in as participant and journey to waiting room
        var individual = hearingDto.Participants[0];
        var individualWaitingRoom = ParticipantLoginToWaitingRoomJourney(individual, hearingDto.CaseNumber);
        
        // log in as JVS endpoint
        var endpoint = hearingDto.VideoAccessPoints[0];
        var pexipNodeAddress = EnvConfigSettings.PexipNodeAddress;
        var pexipWebAppUrl =
            PexipWebAppUrlBuilder.BuildPexipWebAppUrl(pexipNodeAddress, _sipUrlAndPin.SipUrl, _sipUrlAndPin.Pin, endpoint.DisplayName);
        var jvsWebPage = LoginAsJvsEndpoint(pexipWebAppUrl, endpoint.DisplayName);
        jvsWebPage.ClickJoinMeetingButton();
        
        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(hearingDto.CaseNumber);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(endpoint.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(individual.FullName);
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeHearingRoomPage.WaitForCountdownToComplete();
        
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.DisplayName).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(individual.DisplayName).Should().BeTrue();
        
        // ensure the participant waiting room content has loaded correctly
        var individualHearingRoom = individualWaitingRoom.TransferToHearingRoom();
        
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();
        individualWaitingRoom = individualHearingRoom.TransferToWaitingRoom();
        
        // ensure participants are transferred back to the waiting room
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(endpoint.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(individual.FullName);
        
        judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeHearingRoomPage.WaitForCountdownToComplete();
        individualHearingRoom = individualWaitingRoom.TransferToHearingRoom();
        
        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();
        individualHearingRoom.TransferToWaitingRoom();
        
        // ensure participants are transferred back to the waiting room
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(endpoint.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(individual.FullName);
        
        Assert.Pass("Smoke test passed");
    }

    /// <summary>
    /// Create a hearing with a judge, one individual and one endpoint
    /// </summary>
    /// <returns></returns>
    private BookingDto CreateABookingForSmokeTest()
    {
        var hearingScheduledDateAndTime = DateUtil
            .GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDto(HearingTestData.JudgePersonalCode,
            HearingTestData.JudgeUsername, scheduledDateTime: hearingScheduledDateAndTime);

        hearingDto.Participants =
            hearingDto.Participants.Where(x => x.Role == GenericTestRole.Applicant).Take(1).ToList();

        hearingDto.VideoAccessPoints = [HearingTestData.CreateNewEndpointDto()];

        hearingDto.CaseName = $"Daily Smoke Test {hearingScheduledDateAndTime:M-d-yy-H-mm-ss} {Guid.NewGuid():N}";
        hearingDto.CaseNumber = $"Automated Daily Smoke Test {Guid.NewGuid():N}";
        
        return hearingDto;
    }
    
    private async Task BookHearing(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
            
        await driver.Navigate().GoToUrlAsync(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();

        var summaryPage = createHearingPage.BookAHearingJourney(bookingDto);
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestHearingIds.Add(_hearingIdString);
        await TestContext.Out.WriteLineAsync("Smoke Test Hearing ID: " + _hearingIdString);
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        _sipUrlAndPin= bookingDetailsPage.GetSipConnectionDetailsAtPosition(1);
        bookingDetailsPage.GoToDashboardPage();
    }
    
    private ParticipantWaitingRoomPage ParticipantLoginToWaitingRoomJourney(BookingParticipantDto participant, string caseNumber)
    {
        var participantUsername = participant.Username;
        var participantPassword = EnvConfigSettings.UserPassword;
        var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative, participant.VideoFileName);
        return JourneyToWaitingRoom(participant, participantHearingList, participantUsername, caseNumber);
    }
    
    private ParticipantWaitingRoomPage JourneyToWaitingRoom(BookingParticipantDto participant,
        ParticipantHearingListPage participantHearingList, string participantUsername, string caseNumber)
    {
        var participantWaitingRoom = participantHearingList
            .SelectHearing(caseNumber).GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
        // store the participant driver in a dictionary, so we can access it later to sign out
        ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        return participantWaitingRoom;
    }
}