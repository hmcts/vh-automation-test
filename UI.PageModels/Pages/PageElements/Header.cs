using OpenQA.Selenium;

namespace UI.PageModels.Pages.PageElements
{
	///<summary>
	/// Common Header elements
	///</summary>
    public class Header
    {
        public static By SignOut = By.Id("logout-link");
        public static By LinkSignOut = By.Id("linkSignOut");
        public static By BookingsList = By.XPath("//ul[@id='navigation']//a[text()[contains(.,'Booking')]]");
        public static By SignoutCompletely = By.CssSelector("div.table-cell.text-left.content");
    }
}
