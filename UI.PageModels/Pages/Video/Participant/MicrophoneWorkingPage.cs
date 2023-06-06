using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class SeeAndHearVideoPage : VhVideoWebPage
{
    private readonly By _continueBtn = By.Id("continue-btn");
    private readonly By _yesRadioButton = By.XPath("//label[normalize-space()='Yes']");

    public SeeAndHearVideoPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public CourtRulesPage SelectYesToVisualAndAudioClarity()
    {
        ClickElement(_yesRadioButton);
        ClickElement(_continueBtn);
        return new CourtRulesPage(Driver, DefaultWaitTime);
    }
}

public class MicrophoneWorkingPage : VhPage
{
    private readonly By _continueBtn = By.Id("continue-btn");
    private readonly By _microphoneYesRadioButton = By.XPath("//label[normalize-space()='Yes']");

    public MicrophoneWorkingPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public SeeAndHearVideoPage SelectMicrophoneYes()
    {
        ClickElement(_microphoneYesRadioButton);
        ClickElement(_continueBtn);
        return new SeeAndHearVideoPage(Driver, DefaultWaitTime);
    }
}