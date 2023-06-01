using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video
{
	///<summary>
	///   HearingRoomPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class HearingRoomPage
    {
        public static By LabelStatusAvailable = By.XPath("//label[contains(@class, 'label-status--available')]");
        public static By ParticipantRole = By.XPath("//p[contains(@class, 'hearing-role-participant')]");
        public static By ParticipantType = By.XPath("//label[contains(@class, 'case-type-group-participant')]");
        public static By PanelList = By.Id("panelList");
        public static By CloseHearingButton = By.Id("end-hearing-desktop");
        public static By LeaveHearingButton = By.Id("leave-hearing-desktop");
        public static By ConfirmCloseHearingButton = By.Id("btnConfirmClose");
        public static By ButtonConfirmLeaveHearing = By.Id("btnConfirmLeave");
        public static By PauseHearing = By.Id("pause-hearing-desktop");
        public static By MuteAndLock => By.XPath("//*[contains(text(),'Mute & lock')]");
        public static By UnlockMute => By.XPath("//*[contains(text(),'Unlock mute')]");
        public static By LowerHands => By.XPath("//*[contains(text(),'Lower hands')]");
        public static By ParticipantDisplayName(string name) => By.XPath($"//*[contains(text(),'{name}')]");
        public static By IncomingFeedJudgeVideo => By.Id("incomingFeedJudgePrivate");
        public static By ParticipantMicLocked => By.Id("toggle-audio-mute-locked-img-desktop");
        public static By ParticipantMicUnlocked => By.Id("toggle-audio-mute-img-desktop");
        public static By MicMutedIcon => By.XPath("//img[@alt='Microphone muted icon']");
        public static By JudgeMicMuted => By.XPath("//div[@id='toggle-audio-mute-img-desktop']//svg[@data-icon='microphone-slash']");
        public static By JudgeMicActive => By.XPath("//div[@id='toggle-audio-mute-img-desktop']//svg[@data-icon='microphone']");
        public static By JudgeYellow => By.XPath("//div[@id='panelList']//div[@class='yellow']");
        public static By ParticipantToggleRaiseHand => By.Id("toggle-hand-raised-img-desktop");
        public static By ParticipantToggleVideo => By.Id("toggle-video-mute-img-desktop");
        public static By ParticipantHandRaised => By.XPath("//div[@colour='grey'][@class='icon-button']//fa-icon[@class='ng-fa-icon yellow']");
        public static By ParticipantCameraOffIcon => By.XPath("//div[@class='icon-button no-click']//fa-icon[@icon='video-slash']");
        public static By ShareScreenButton => By.Id("start-screenshare-img-desktop");
        public static By StopSharingScreen => By.Id("stop-screenshare-img-desktop");
        public static By ShareDocuments => By.Id("shareDropdown-startDocumentSharing");
        public static By OutgoingScreenShare => By.ClassName("outgoing-present-video-container");
        public static By SecondVideoFeed => By.XPath("//div[@id='secondIncomingFeed']");
        public static By ParticipantPanelToggel => By.Id("toggle-participants-panel");
        public static By ChatPanel => By.Id("toggle-chat-panel-desktop");
        public static By ChatInputBox => By.Id("new-message-box");
        public static By ChatSendMessageButton => By.Id("send-new-message-btn");
        public static By ChatSentMessageItem => By.ClassName("message-item message-item-sent");
        public static By ChatList => By.Id("chat-list");
        public static By JudgeMessageSent(string message) => By.XPath($"//div[text()[contains(.,'{message}')]]");
    }
}
