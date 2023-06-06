using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
    public class JudgeWaitingRoomPage : VhVideoWebPage
	{
		private readonly By _startHearingBtn = By.XPath("//button[normalize-space()='Start video hearing']");
        // public static readonly By ReturnToHearingRoomLink = By.XPath("//a[contains(text(),'Return to video hearing list')]");
        // public static readonly By HearingTitle = By.XPath("//*[contains(text(),'case number')]//ancestor::td");
        // public static readonly By HearingDateTime = By.XPath("//span[contains(text(),'to')]/ancestor::td");
        // public static readonly By ChooseCameraMicrophoneButton = By.Id("changeCameraButton");
        // public static readonly By CloseChangeDeviceButton = By.Id("change-device-btn");
        // public static readonly By ConfirmStartHearingButton = By.Id("btnConfirmStart");
        // public static readonly By CancelStartHearingButton = By.Id("btnCancelStart");
        // public static readonly By EnterPrivateConsultationButton = By.Id("joinPCButton");
        // public static readonly By NumberOfJohsInConsultaionRoom = By.Id("numberOfJohsInConsultationBadge");
        // private static readonly By NumberOfJohsInConsultationBadge = By.Id("numberOfJohsInConsultationBadge");
        // public static By ResumeVideoHearing => By.XPath("//button[text()[contains(.,'Resume video hearing')]]");
        // public static By ToastInviteAcceptButton => By.Id("notification-toastr-invite-accept");

        public JudgeWaitingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
	        WaitForElementToBeClickable(_startHearingBtn);
        }
    }
}
