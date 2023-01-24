using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   OtherInformationPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class OtherInformationPage
    {
        public static By RecordAudioYes = By.Id("audio-choice-yes");
        public static By RecordAudioNo = By.Id("audio-choice-no");
        public static By OtherInfo = By.Id("details-other-information");
        public static By NextButton = By.Id(("nextButton"));
    }
}
