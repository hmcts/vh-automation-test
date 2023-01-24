using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   JoinYourHearingPage 
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class JoinYourHearingPage
    {
        public static By FullName => By.Id("full-name");
        public static By QuickLinkParticipant => By.CssSelector("#QuickLinkParticipant-item-title");
        public static By QuickLinkObserver => By.Id("QuickLinkObserver");
        public static By ContinueButton => By.Id("continue-button");
    }
}
