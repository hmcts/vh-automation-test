using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class TestingEquipmentPage : VhVideoWebPage
{
    private readonly By _continueBtn = By.Id("continue-btn");

    public TestingEquipmentPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_continueBtn);
    }

    public CameraWorkingPage GoToCameraWorkingPage()
    {
        ClickElement(_continueBtn);
        return new CameraWorkingPage(Driver, DefaultWaitTime);
    }
}