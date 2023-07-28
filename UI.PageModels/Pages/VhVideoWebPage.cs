using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages;

public abstract class VhVideoWebPage : VhPage
{
    private readonly By _signOutMenuItemButton = By.Id("logout-link");

    protected VhVideoWebPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public VideoWebLogoutPage SignOut(bool confirmSignOut = true)
    {
        ClickElement(_signOutMenuItemButton);
        var confirmSignOutLocator = By.XPath("//*[@id='tilesHolder']/div");
        if (confirmSignOut)
        {
            ClickElement(confirmSignOutLocator);
        }

        return new VideoWebLogoutPage(Driver, DefaultWaitTime);
    }
}