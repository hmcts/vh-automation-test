using OpenQA.Selenium;

namespace UI.PageModels.Pages
{
    public class VideoAccessPointsPage : VhPage
    {
        private static By DisplayName(int number) => By.Id($"displayName{number}");
        private static By DefenceAdvocate(int number) => By.Id($"defenceAdvocate{number}");
        private static By RemoveDisplayName(int number) => By.Id($"removeDisplayName{number}");
        private readonly By _addAnotherBtn = By.Id("addEndpoint");
        private readonly By _nextButton = By.Id(("nextButton"));

        public VideoAccessPointsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
	        WaitForElementToBeVisible(_addAnotherBtn);
        }

        public OtherInfoPage GoToNextPage()
        {
	        ClickElement(_nextButton);
	        return new OtherInfoPage(Driver, DefaultWaitTime);
        }
    }
}
