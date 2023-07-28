using UI.PageModels.Pages.Video.Participant;

namespace UI.NUnitVersion.Video;

public class QuickLinkUserTests : VideoWebUiTest
{
    private string _quickLinkJoinUrl;

    [Test]
    public void JoinAHearingAsAQuickLinkUser()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDtoWithOnlyAJudge(scheduledDateTime:hearingScheduledDateAndTime);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        
        BookHearing(hearingDto);
        
        // log in as judge and start the hearing
        var judgeUsername = hearingDto.Judge.Username;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(hearingDto.CaseName);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;

        var quickLinkName1 = $"QL Auto Join 1 {Guid.NewGuid():N}";
        var quickLinkName2 = $"QL Auto Join 2 {Guid.NewGuid():N}";
        var qlWaitingRoomPage1 = LoginInAsQlAndNavigateToWaitingRoom(quickLinkName1, hearingDto.CaseName);
        var qlWaitingRoomPage2 = LoginInAsQlAndNavigateToWaitingRoom(quickLinkName2, hearingDto.CaseName);
        
        var ql1ConsultationPage = qlWaitingRoomPage1.StartPrivateConsultation(new List<string>() {quickLinkName2});
        var ql2ConsultationPage = qlWaitingRoomPage2.AcceptPrivateConsultation();
        
        ql1ConsultationPage.IsParticipantConnected(quickLinkName2).Should().BeTrue();
        ql2ConsultationPage.IsParticipantConnected(quickLinkName1).Should().BeTrue();
        
        judgeWaitingRoomPage.GetParticipantStatus(quickLinkName1).Should().BeEquivalentTo("In Consultation");
        judgeWaitingRoomPage.GetParticipantStatus(quickLinkName2).Should().BeEquivalentTo("In Consultation");
        
        // need to set the original waiting room object to the current page else the driver will not be able to navigate sign out
        qlWaitingRoomPage1 = ql1ConsultationPage.LeaveConsultationRoom();
        qlWaitingRoomPage2 = ql2ConsultationPage.LeaveConsultationRoom();

        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();

        // sign out of each hearing
        SignOutAllUsers();
        
        Assert.Pass();
    }

    private ParticipantWaitingRoomPage LoginInAsQlAndNavigateToWaitingRoom(string qlName, string caseName)
    {
        var quickLinkJoinHearingPage = LoginAsQuickLinkUser(_quickLinkJoinUrl, qlName);
        quickLinkJoinHearingPage.EnterQuickLinkUserDetails(qlName, true);
        var quickLinkHearingListPage = quickLinkJoinHearingPage.Continue();
        var page = quickLinkHearingListPage.SelectHearing(caseName).GoToEquipmentCheck()
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

        createHearingPage.EnterHearingDetails(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType,
            bookingDto.HearingType);

        var hearingSchedulePage = createHearingPage.GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(bookingDto.ScheduledDateTime, bookingDto.DurationHour,
            bookingDto.DurationMinute, bookingDto.VenueName, bookingDto.RoomName);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(bookingDto.Judge.Username, bookingDto.Judge.DisplayName, bookingDto.Judge.Phone);

        var addParticipantPage = assignJudgePage.GoToParticipantsPage();
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(bookingDto.OtherInformation);

        var summaryPage = otherInformationPage.GoToSummaryPage();
        var confirmationPage = summaryPage.ClickBookButton();
        var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
        _quickLinkJoinUrl = bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl);
        TestContext.WriteLine(_quickLinkJoinUrl);
    }
}