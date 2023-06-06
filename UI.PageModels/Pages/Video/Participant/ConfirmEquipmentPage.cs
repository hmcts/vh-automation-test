using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
    public class ConfirmEquipmentPage : VhVideoWebPage
    {
        public static By ContinueBtn => By.Id("continue-btn");

        public ConfirmEquipmentPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
        }
    }
}