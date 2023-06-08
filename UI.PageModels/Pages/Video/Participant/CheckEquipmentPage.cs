using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class CheckEquipmentPage : VhVideoWebPage
{
    private readonly By _continue = By.Id("continue-btn");

    public CheckEquipmentPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public SwitchOnCameraMicrophonePage GoToSwitchOnCameraMicrophonePage()
    {
        ClickElement(_continue);
        return new SwitchOnCameraMicrophonePage(Driver, DefaultWaitTime);
    }
}