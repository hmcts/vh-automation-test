using OpenQA.Selenium;

namespace UI.PageModels.Pages;

public abstract class VhVideoWebPage : VhPage
{
    private readonly By _signOutMenuItemButton = By.Id("logout-link");
    
    protected VhVideoWebPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    public void SignOut()
    {
        ClickElement(_signOutMenuItemButton);
    }
}