using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class WatchtheVideoPage : VhVideoWebPage
{
    public WatchtheVideoPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By WatchVideoButton => By.Id("watch-video-btn");
}