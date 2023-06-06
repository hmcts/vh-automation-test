using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
    public class SwitchOnCameraMicrophonePage : VhVideoWebPage
    {
        private readonly By _switchOnButton = By.Id("switch-on-btn");
        private readonly By _switchOnCameraMicrophoneTitle = By.XPath("//h1[normalize-space()='Your camera and microphone are switched on']");
        private readonly By _watchVideoBtn = By.Id("watch-video-btn");

        public SwitchOnCameraMicrophonePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
        }

        public TestingEquipmentPage SwitchOnCameraMicrophone()
        {
            ClickElement(_switchOnButton);
            WaitForElementToBeVisible(_switchOnCameraMicrophoneTitle);
            ClickElement(_watchVideoBtn);
            return new TestingEquipmentPage(Driver, DefaultWaitTime);
        }
    }
}