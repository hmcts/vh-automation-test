namespace UI.PageModels.Pages.Video;

public class VideoWebLogoutPage(IWebDriver driver, int defaultWaitTime)
    : VhPage(driver, defaultWaitTime, useAltLocator: false)
{
    private readonly By _signInBtn = By.XPath("//a[normalize-space()='here']"); // link to sign in page

    public VideoWebLoginPage ClickSignIn()
    {
        ClickElement(_signInBtn);
        return new VideoWebLoginPage(Driver, DefaultWaitTime);
    }
}