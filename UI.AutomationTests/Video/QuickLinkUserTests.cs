using UI.AutomationTests.TestData;
using UI.AutomationTests.Utilities;
using UI.PageModels.Pages.Video.Participant;

namespace UI.AutomationTests.Video;

[Category("Daily")]
public class QuickLinkUserTests : VideoWebUiTest
{
    private string _quickLinkJoinUrl;
    private string _hearingIdString;

    [Test]
    public async Task JoinAHearingAsAQuickLinkUser()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDtoWithOnlyAJudge(scheduledDateTime:hearingScheduledDateAndTime);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        BookHearing(hearingDto);
        var conference = await GetConference(new Guid(_hearingIdString));
        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;

        var quickLinkName1 = $"QL Auto Join 1 {Guid.NewGuid():N}";
        var quickLinkName2 = $"QL Auto Join 2 {Guid.NewGuid():N}";
        
        var qlWaitingRoomPage1 = LoginInAsQlAndNavigateToWaitingRoom(quickLinkName1, conference.Id.ToString());
        var qlWaitingRoomPage2 = LoginInAsQlAndNavigateToWaitingRoom(quickLinkName2, conference.Id.ToString());

        var particiantsFromConference = await VideoApiClient.GetParticipantsByConferenceIdAsync(conference.Id);
        var quicklink1 = particiantsFromConference.First(x => x.DisplayName == quickLinkName1);
        var quicklink2 = particiantsFromConference.First(x => x.DisplayName == quickLinkName2);
        
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quicklink1.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quicklink2.DisplayName);
        judgeWaitingRoomPage.ClearParticipantAddedNotification();
        
        var ql1ConsultationPage = qlWaitingRoomPage1.StartPrivateConsultation(new List<string>() {quicklink2.DisplayName});
        var ql2ConsultationPage = qlWaitingRoomPage2.AcceptPrivateConsultation();
        
        ql1ConsultationPage.IsParticipantConnected(quicklink2.DisplayName).Should().BeTrue();
        ql2ConsultationPage.IsParticipantConnected(quicklink1.DisplayName).Should().BeTrue();
        
        judgeWaitingRoomPage.GetParticipantStatus(quicklink1.DisplayName).Should().BeEquivalentTo("In Consultation");
        judgeWaitingRoomPage.GetParticipantStatus(quicklink2.DisplayName).Should().BeEquivalentTo("In Consultation");
        
        // need to set the original waiting room object to the current page else the driver will not be able to navigate sign out
        qlWaitingRoomPage1 = ql1ConsultationPage.LeaveConsultationRoom();
        qlWaitingRoomPage2 = ql2ConsultationPage.LeaveConsultationRoom();

        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quicklink1.DisplayName);
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(quicklink2.DisplayName);
        
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();

        judgeHearingRoomPage.AdmitParticipant(quicklink1.DisplayName, quicklink1.Id.ToString()); 
        judgeHearingRoomPage.AdmitParticipant(quicklink2.DisplayName, quicklink2.Id.ToString());
        
        var qlHearingRoom1 = qlWaitingRoomPage1.TransferToHearingRoom();
        var qlHearingRoom2 = qlWaitingRoomPage2.TransferToHearingRoom();
        
        judgeHearingRoomPage.IsParticipantInHearing(quicklink1.DisplayName).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(quicklink2.DisplayName).Should().BeTrue();

        judgeHearingRoomPage.DismissParticipant(quicklink1.DisplayName);
        judgeHearingRoomPage.DismissParticipant(quicklink2.DisplayName);

        qlHearingRoom1.TransferToWaitingRoom();
        qlHearingRoom2.TransferToWaitingRoom();
        
        judgeHearingRoomPage.IsParticipantInHearing(quicklink1.DisplayName).Should().BeFalse();
        judgeHearingRoomPage.IsParticipantInHearing(quicklink2.DisplayName).Should().BeFalse();
        
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
        var summaryPage = createHearingPage.BookAHearingJourney(bookingDto, FeatureToggles.UseV2Api());
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestHearingIds.Add(_hearingIdString);
        TestContext.WriteLine($"Hearing  ID: {_hearingIdString}");
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        _quickLinkJoinUrl = bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl);
        TestContext.WriteLine(_quickLinkJoinUrl);
    }
}