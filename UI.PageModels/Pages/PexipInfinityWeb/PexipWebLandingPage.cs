namespace UI.PageModels.Pages.PexipInfinityWeb;

public class PexipWebAppPage(IWebDriver driver, int defaultWaitTime)
    : VhPage(driver, defaultWaitTime, useAltLocator: false)
{
    private readonly By _joinMeetingButton = By.CssSelector("button[type='submit']");
    private readonly By _leaveHearingButton = By.CssSelector("button[aria-label='Leave meeting']");

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_joinMeetingButton);
    }
    
    public void ClickJoinMeetingButton()
    {
        ClickElement(_joinMeetingButton);
    }
}

public static class PexipWebAppUrlBuilder
{
    public static string BuildPexipWebAppUrl(string pexipNodeAddress, string sipAddress, string pin, string displayName = "VH Test")
    {
        return $"https://{pexipNodeAddress}/webapp3/m/{sipAddress}?pin={pin}&name={displayName}&join=1";
    }
}