namespace UI.NUnitVersion.Video;

public class EndToEndTest : VideoWebUiTest
{
    [Test]
    [Ignore("Need to setup the booking first and decide on managing the e2e")]
    public void BookAHearingAndLogInAsJudgeAndParticipants()
    {
        var caseName = "Booking E2E test";
        var judgeUsername = "";
        var judgePassword = "";
        var judgeHearingListPage = LoginAsJudge(judgeUsername, judgePassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(caseName);

        // loop through all participants in hearing and login as each one
        var participantUsername = "";
        var participantPassword = "";
        var participantHearingList = LoginAsParticipant(participantUsername, participantPassword);
        var participantWaitingRoom = participantHearingList.SelectHearing(caseName).GoToEquipmentCheck().GoToSwitchOnCameraMicrophonePage()
            .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
            .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration();

    }
}