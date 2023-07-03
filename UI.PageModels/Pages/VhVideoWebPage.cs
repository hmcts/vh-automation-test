using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages;

public abstract class VhVideoWebPage : VhPage
{
    private readonly By _signOutMenuItemButton = By.Id("logout-link");

    protected VhVideoWebPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public VideoWebLogoutPage SignOut()
    {
        ClickElement(_signOutMenuItemButton);
        ClickElement(By.XPath("//*[@id='tilesHolder']/div"));
        return new VideoWebLogoutPage(Driver, DefaultWaitTime);
    }
}