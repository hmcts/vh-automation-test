using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   SummaryPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class SummaryPage
    {
        public static By BookButton = By.Id("bookButton");
        public static By DotLoader = By.Id("dot-loader");
        public static By TryAgainButton = By.Id("btnTryAgain");
        public static By SuccessTitle = By.XPath("//h1[text()[contains(.,'Your hearing booking was successful')]]");
        public static By ViewThisBooking = By.LinkText("View this booking");
    }
}
