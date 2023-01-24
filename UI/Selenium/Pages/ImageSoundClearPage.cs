using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   ImageSoundClearPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class ImageSoundClearPage
    {
        public static By VideoYesRadioButton => By.CssSelector("label.govuk-label.govuk-radios__label");
        public static By VideoNoRadioButton => By.Id("video-no");
        public static By Continue = By.Id("continue-btn");
    }
}