using UI.PageModels.Pages.Video.Participant;

namespace UI.NUnitVersion.Video;

[Category("Daily")]
public class QuickLinkUserTests : VideoWebUiTest
{
    private string _quickLinkJoinUrl;
    private string _hearingIdString;

    [Test]
    public void JoinAHearingAsAQuickLinkUser()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDtoWithOnlyAJudge(scheduledDateTime:hearingScheduledDateAndTime);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        BookHearing(hearingDto);
        var conference = VideoApiClient.GetConferenceByHearingRefIdAsync(new Guid(_hearingIdString) , false).Result;

        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;

        var quickLinkName1 = $"QL Auto Join 1 {Guid.NewGuid():N}";
        var quickLinkName2 = $"QL Auto Join 2 {Guid.NewGuid():N}";
        
        var qlWaitingRoomPage1 = LoginInAsQlAndNavigateToWaitingRoom(quickLinkName1, conference.Id.ToString());
        judgeWaitingRoomPage.ClearParticipantAddedNotification(quickLinkName1);
        
        var qlWaitingRoomPage2 = LoginInAsQlAndNavigateToWaitingRoom(quickLinkName2, conference.Id.ToString());
        judgeWaitingRoomPage.ClearParticipantAddedNotification(quickLinkName2);

        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quickLinkName1);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quickLinkName2);
        
        var ql1ConsultationPage = qlWaitingRoomPage1.StartPrivateConsultation(new List<string>() {quickLinkName2});
        var ql2ConsultationPage = qlWaitingRoomPage2.AcceptPrivateConsultation();
        
        ql1ConsultationPage.IsParticipantConnected(quickLinkName2).Should().BeTrue();
        ql2ConsultationPage.IsParticipantConnected(quickLinkName1).Should().BeTrue();
        
        judgeWaitingRoomPage.GetParticipantStatus(quickLinkName1).Should().BeEquivalentTo("In Consultation");
        judgeWaitingRoomPage.GetParticipantStatus(quickLinkName2).Should().BeEquivalentTo("In Consultation");
        
        // need to set the original waiting room object to the current page else the driver will not be able to navigate sign out
        qlWaitingRoomPage1 = ql1ConsultationPage.LeaveConsultationRoom();
        qlWaitingRoomPage2 = ql2ConsultationPage.LeaveConsultationRoom();

        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quickLinkName1);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quickLinkName2);
        
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();

        judgeHearingRoomPage.AdmitParticipant(quickLinkName1); 
        judgeHearingRoomPage.AdmitParticipant(quickLinkName2);
        
        var qlHearingRoom1 = qlWaitingRoomPage1.TransferToHearingRoom();
        var qlHearingRoom2 = qlWaitingRoomPage2.TransferToHearingRoom();
        
        judgeHearingRoomPage.IsParticipantInHearing(quickLinkName1).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(quickLinkName2).Should().BeTrue();

        judgeHearingRoomPage.DismissParticipant(quickLinkName1);
        judgeHearingRoomPage.DismissParticipant(quickLinkName2);

        qlHearingRoom1.TransferToWaitingRoom();
        qlHearingRoom2.TransferToWaitingRoom();
        
        judgeHearingRoomPage.IsParticipantInHearing(quickLinkName1).Should().BeFalse();
        judgeHearingRoomPage.IsParticipantInHearing(quickLinkName2).Should().BeFalse();
        
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();

        // sign out of each hearing
        SignOutAllUsers();
        
        Assert.Pass();
    }

    private ParticipantWaitingRoomPage LoginInAsQlAndNavigateToWaitingRoom(string qlName, string conferenceId)
    {
        var quickLinkJoinHearingPage = LoginAsQuickLinkUser(_quickLinkJoinUrl, qlName);
        quickLinkJoinHearingPage.EnterQuickLinkUserDetails(qlName, true);
        var quickLinkHearingListPage = quickLinkJoinHearingPage.Continue();
        var page = quickLinkHearingListPage.SelectHearing(conferenceId).GoToEquipmentCheck()
            .GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration();
        
        ParticipantDrivers[qlName].VhVideoWebPage = page;
        
        return page;
    }
    
    private void BookHearing(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();
        var summaryPage = createHearingPage.EnterHearingDetails(bookingDto, FeatureToggles.UseV2Api());
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestContext.WriteLine($"Hearing  ID: {_hearingIdString}");
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        _quickLinkJoinUrl = bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl);
        TestContext.WriteLine(_quickLinkJoinUrl);
    }
    
    protected override async Task CleanUp()
    {
        if(_hearingIdString != null)
        {
            var hearingId = Guid.Parse(_hearingIdString);
            await BookingsApiClient.RemoveHearingAsync(hearingId);
            TestContext.WriteLine($"Removed Hearing {hearingId}");
            _hearingIdString = null;
        }
    }
}