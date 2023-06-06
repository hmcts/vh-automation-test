using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
	///<summary>
	///   ConsultationRoomPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class ConsultationRoomPage : VhVideoWebPage
    {
        public static By InviteParticipant(string name) => By.XPath($"//div[@class='participant-endpoint-row' and contains(.,'{name}')]//app-invite-participant");
        public static By InviteParticipants => By.ClassName("phone");
        public static By ParticipantsTick => By.CssSelector(".member-group+.member-group .yellow fa-icon");
        public static By ParticipantDisplayName(string name) => By.XPath($"//div[@class='participant-endpoint-row' and contains(.,'{name}')]");
        public static By ConfirmLeaveButton => By.Id("consultation-leave-button");
        public static By CloseButton => By.Id("closeButton");
        public static By LeaveButtonDesktop => By.Id("leaveButton-desktop");
        public static By WaitingRoomIframe => By.Id("admin-frame");
        public static By WaitingRoomJudgeLink => By.XPath("//table[@id='WaitingRoom']//td[contains(.,'Judge')]");
        public static By WaitingRoomParticipantLink => By.XPath("//td[contains(@id,'-WaitingRoom-menu')][not(text(),'Judge')]");
        public static By WaitingRoomMenu => By.XPath("//ul[contains(@id,'-WaitingRoom-menu')]");
        public static By PrivateConsultation => By.XPath("//ul[contains(@id,'-WaitingRoom-menu')]//li");
        public static By HearingListConsultationRooms = By.Id("ConsultationRooms");
        public static By SelfViewButton => By.Id("selfViewButton");
        public static By MuteButton => By.Id("muteButton");

        public ConsultationRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
        }
    }
}
