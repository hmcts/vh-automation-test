using FluentAssertions;
using UI.NUnitVersion.TestData;
using UI.NUnitVersion.Utilities;
using UI.PageModels.Dtos;

namespace UI.NUnitVersion.Video;

public class EndToEndTest : VideoWebUiTest
{
    [Test]
    public void BookAHearingAndLogInAsJudgeAndParticipants()
    {
        var hearingDto = HearingTestData.CreateHearingDto();
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        hearingDto.ScheduledDateTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
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
        var judgeUsername = hearingDto.Judge.Username;
        var judgePassword = EnvConfigSettings.UserPassword;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(hearingDto.CaseName);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;

        // confirm all participants are connected
        judgeWaitingRoomPage.GetParticipantConnectedCount().Should().Be(hearingDto.Participants.Count);

        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();

        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();

        // sign out of each hearing
        SignOutAllUsers();
        
        Assert.Pass();
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
    }
}