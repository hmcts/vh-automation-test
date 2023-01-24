using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   ParticipantHearingListPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class ParticipantHearingListPage
    {
        public static By CheckEquipment = By.Id("check-equipment-btn");
        public static By HearingListPageTitle = By.XPath("//*[contains(text(), 'Video hearings for') or contains(text(),'Your video hearing') or contains(text(),'Your video hearings')]");
        public static By TestingYourEquipment = By.XPath("//*[contains(text(), ' Testing your equipment')]");
        public static By SignInTime(string conferenceId) => By.Id($"participant-sign-in-time-{conferenceId}");
        public static By SignInButton(string conferenceId) => By.Id($"sign-into-hearing-btn-{conferenceId}");
        public static By HealingListRow => By.XPath("//tr[@class='govuk-table__row']");
        public static By SelectButton(string caseId) => By.XPath($"//tr[contains(.,'{caseId}')]//button");
        public static By ButtonNext => By.Id("next");
        public static By ContinueButton => By.Id("continue-btn");
        public static By SwitchOnButton => By.Id("switch-on-btn");
        public static By WatchVideoButton => By.Id("watch-video-btn");
        public static By CameraWorkingYes => By.XPath("//label[@for='camera-yes']");
        public static By CameraWorkingNo => By.XPath("//label[@for='camera-no']");
        public static By MicrophoneWorkingYes => By.Id("microphone-yes");
        public static By MicrophoneWorkingNo => By.Id("microphone-no");
        public static By VideoWorkingYes => By.Id("video-yes");
        public static By VideoWorkingNo => By.Id("video-no");
        public static By NextButton => By.Id("nextButton");
        public static By DeclareCheckbox => By.Id("declare");
        public static By IncomingStreamVideo => By.Id("incomingStream");
        public static By OutgoingStreamVideo => By.Id("outgoingStream");
        public static By Meter => By.Id("meter");
    }
}
