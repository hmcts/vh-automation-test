using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class CheckEquipmentPage : VhVideoWebPage
{
    private readonly By _continue = By.Id("continue-btn");

    public CheckEquipmentPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeVisible(By.XPath("//h1[contains(text(), 'confirm your equipment is working')]"));
    }

    public SwitchOnCameraMicrophonePage GoToSwitchOnCameraMicrophonePage()
    {
        ConfirmPageHasLoaded();
        ClickElement(_continue);
        return new SwitchOnCameraMicrophonePage(Driver, DefaultWaitTime);
    }
}