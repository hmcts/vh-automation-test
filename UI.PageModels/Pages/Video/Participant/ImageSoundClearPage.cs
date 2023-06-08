using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class ImageSoundClearPage : VhVideoWebPage
{
    public static By Continue = By.Id("continue-btn");

    public ImageSoundClearPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By VideoYesRadioButton => By.CssSelector("label.govuk-label.govuk-radios__label");
    public static By VideoNoRadioButton => By.Id("video-no");
}