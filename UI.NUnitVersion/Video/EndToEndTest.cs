using UI.NUnitVersion.TestData;
using UI.PageModels.Dtos;

namespace UI.NUnitVersion.Video;

public class EndToEndTest : VideoWebUiTest
{
    [Test]
    [Ignore("Need to setup the booking first and decide on managing the e2e")]
    public void BookAHearingAndLogInAsJudgeAndParticipants()
    {
        var hearingDto = HearingTestData.CreateHearingDto();
        BookHearing(hearingDto);
        
        // loop through all participants in hearing and login as each one
        foreach (var participant in hearingDto.Participants)
        {
            var participantUsername = participant.Username;
            var participantPassword = EnvConfigSettings.UserPassword;
            var participantHearingList = LoginAsParticipant(participantUsername, participantPassword);
            var participantWaitingRoom = participantHearingList.SelectHearing(hearingDto.CaseName).GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration();
            // store the participant driver in a dictionary so we can access it later to sign out
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        }

        // log in as judge and start the hearing
        var judgeUsername = "";
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(hearingDto.CaseName);
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartHearing();

        // confirm all participants are connected
        foreach (var participant in hearingDto.Participants)
        {
            judgeHearingRoomPage.ConfirmParticipantConnected(participant.Username);
        }

        judgeHearingRoomPage.CloseHearing();

        // sign out of each hearing
        foreach (var videoWebParticipant in ParticipantDrivers.Values)
        {
            videoWebParticipant.VhVideoWebPage.SignOut();
        }
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
        assignJudgePage.EnterJudgeDetails("auto_aw.judge_02@hearings.reform.hmcts.net", "Auto Judge", "");
        
        var addParticipantPage = assignJudgePage.GoToParticipantsPage();
        addParticipantPage.AddExistingParticipants(bookingDto.Participants);
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation("This is a test info");
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        var confirmationPage = summaryPage.ClickBookButton();
        confirmationPage.ClickViewBookingLink();
        confirmationPage.SignOut();
    }
}