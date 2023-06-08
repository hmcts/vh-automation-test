using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class CameraWorkingPage : VhVideoWebPage
{
    private readonly By _cameraYesRadioButton = By.XPath("//label[normalize-space()='Yes']");
    private readonly By _continue = By.Id("continue-btn");

    public CameraWorkingPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public MicrophoneWorkingPage SelectCameraYes()
    {
        ClickElement(_cameraYesRadioButton);
        ClickElement(_continue);
        return new MicrophoneWorkingPage(Driver, DefaultWaitTime);
    }
}