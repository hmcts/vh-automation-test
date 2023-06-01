using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video
{
	///<summary>
	///   MicroPhoneWorkingPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class MicroPhoneWorkingPage
    {
        public static By MicrophoneYesRadioBUtton => By.CssSelector("label.govuk-label.govuk-radios__label");
        public static By MicrophoneNoRadioBUtton => By.Id("microphone-no");
        public static By Continue = By.Id("continue-btn");
    }
}