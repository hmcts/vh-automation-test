namespace UI.PageModels.Pages.Video.Participant;

public class TestingEquipmentPage : VhVideoWebPage
{
    private readonly By _continueBtn = By.Id("continue-btn");

    public TestingEquipmentPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeInvisible(By.XPath("//p[normalize-space()='Please wait to be connected...']"), DefaultWaitTime);
        WaitForElementToBeClickable(_continueBtn);
    }

    public CameraWorkingPage GoToCameraWorkingPage(bool waitForSelfTestToComplete = false)
    {
        if (waitForSelfTestToComplete)
        {
            var videoStream = By.Id("incomingStream");
            WaitForElementToBeVisible(videoStream);
            WaitForElementToBeInvisible(videoStream, DefaultWaitTime);
        }
        ClickElement(_continueBtn);
        return new CameraWorkingPage(Driver, DefaultWaitTime);
    }
}