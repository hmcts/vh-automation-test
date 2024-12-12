using UI.PageModels.Pages.Video.Participant;

namespace UI.AutomationTests.Video;

public class QuickLinkUserTests : VideoWebUiTest
{
    private string _quickLinkJoinUrl;

    [Test]
    [Category("video")]
    public async Task JoinAHearingAsAQuickLinkUser()
    {
        var hearing = await CreateTestHearing();
        TestHearingIds.Add(hearing.Id.ToString());
        await TestContext.Out.WriteLineAsync(
            $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");
        await GetQuicklinkJoinUri(hearing);
        var conference = await GetConference(hearing.Id);
        // log in as judge and start the hearing
        var judgeUsername = hearing.JudicialOfficeHolders[0].Email;
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
        
        var ql1ConsultationPage = qlWaitingRoomPage1.StartPrivateConsultation([quicklink2.DisplayName]);
        var ql2ConsultationPage = qlWaitingRoomPage2.AcceptPrivateConsultation();
        
        ql1ConsultationPage.IsParticipantConnected(quicklink2.DisplayName).Should().BeTrue();
        ql2ConsultationPage.IsParticipantConnected(quicklink1.DisplayName).Should().BeTrue();
        
        judgeWaitingRoomPage.GetParticipantStatus(quicklink1.Id).Should().BeEquivalentTo("In Consultation");
        judgeWaitingRoomPage.GetParticipantStatus(quicklink2.Id).Should().BeEquivalentTo("In Consultation");
        
        // need to set the original waiting room object to the current page else the driver will not be able to navigate sign out
        qlWaitingRoomPage1 = ql1ConsultationPage.LeaveConsultationRoom();
        qlWaitingRoomPage2 = ql2ConsultationPage.LeaveConsultationRoom();
        
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();

        judgeHearingRoomPage.AdmitParticipant(quicklink1.DisplayName, quicklink1.Id.ToString()); 
        judgeHearingRoomPage.AdmitParticipant(quicklink2.DisplayName, quicklink2.Id.ToString());
        
        var qlHearingRoom1 = qlWaitingRoomPage1.TransferToHearingRoom();
        var qlHearingRoom2 = qlWaitingRoomPage2.TransferToHearingRoom();
        
        judgeHearingRoomPage.IsParticipantInHearing(quicklink1.Id).Should().BeTrue();
        judgeHearingRoomPage.IsParticipantInHearing(quicklink2.Id).Should().BeTrue();

        judgeHearingRoomPage.DismissParticipant(quicklink1.DisplayName, quicklink1.Id.ToString());
        judgeHearingRoomPage.DismissParticipant(quicklink2.DisplayName, quicklink2.Id.ToString());

        qlHearingRoom1.TransferToWaitingRoom();
        qlHearingRoom2.TransferToWaitingRoom();
        
        judgeHearingRoomPage.IsParticipantInHearing(quicklink1.Id).Should().BeFalse();
        judgeHearingRoomPage.IsParticipantInHearing(quicklink2.Id).Should().BeFalse();
        
        judgeHearingRoomPage.WaitForCountdownToComplete();
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();

        // sign out of each hearing
        SignOutAllUsers();
        
        Assert.Pass();
    }
    
    private async Task<HearingDetailsResponseV2> CreateTestHearing()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: hearingScheduledDateAndTime);
        return await BookingsApiClient.BookNewHearingWithCodeAsync(hearingDto);
    }
    
    private async Task GetQuicklinkJoinUri(HearingDetailsResponseV2 hearing)
    {
        var driver = AdminWebDriver.GetDriver();
        await driver.Navigate().GoToUrlAsync(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        var bookingListPage = dashboardPage.GoToBookingList();
        bookingListPage.SearchForBooking(new BookingListQueryDto {CaseNumber = hearing.Cases[0].Number});
        var bookingDetailsPage = bookingListPage.ViewBookingDetails(hearing.Cases[0].Number);
        _quickLinkJoinUrl = bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl);
        bookingDetailsPage.SignOut();
        await TestContext.Out.WriteLineAsync(_quickLinkJoinUrl);
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
}