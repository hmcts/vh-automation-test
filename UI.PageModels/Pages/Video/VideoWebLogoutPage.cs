using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video;

public class VideoWebLogoutPage : VhPage
{
    private readonly By _signInBtn = By.XPath("//a[normalize-space()='here']"); // link to sign in page
    public VideoWebLogoutPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_signInBtn);
    }
    
    public VideoWebLoginPage ClickSignIn()
    {
        ClickElement(_signInBtn);
        return new VideoWebLoginPage(Driver, DefaultWaitTime);
    }
}