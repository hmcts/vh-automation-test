using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
	///<summary>
	///   Which page does this snippet belong to?
	///</summary>
    public class SelectHearingPage : VhVideoWebPage
    {
        public static By Quicklinks => By.CssSelector("fa-icon");
        public static By QuicklinkCopy => By.XPath("//a[text()='Copy join by Video Hearing link details to clipboard']");
        public static By Hearingidtoclipboard => By.XPath("//a[text()='Copy hearing ID to clipboard']");
        public static By Phonetoclipboard => By.XPath("//a[text()='Copy joining by phone details to clipboard']");
        public static By filters => By.CssSelector("#filters-court-rooms");
        public static By NewMessageBox => By.Id("new-message-box");
        public static By UnreadMsgBtn => By.XPath("//app-unread-messages/img");
        public static By UnreadMsgPartBtn => By.XPath("//app-unread-messages-participant/img");

        public SelectHearingPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
        }
    }
}