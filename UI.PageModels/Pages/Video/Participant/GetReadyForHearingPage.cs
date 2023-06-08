using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

/// <summary>
///     Which page does this snippet belong to?
/// </summary>
public class GetReadyForHearingPage : VhVideoWebPage
{
    public GetReadyForHearingPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By Quicklinks => By.CssSelector("fa-icon");
}