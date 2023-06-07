using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

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