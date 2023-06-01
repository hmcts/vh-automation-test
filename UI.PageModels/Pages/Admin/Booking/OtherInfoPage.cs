using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.Booking
{
	public class OtherInfoPage : VhPage
    {
        private readonly By _recordAudioYes = By.Id("audio-choice-yes");
        private readonly By _recordAudioNo = By.Id("audio-choice-no");
        private readonly By _otherInfo = By.Id("details-other-information");
        private readonly By _nextButton = By.Id(("nextButton"));

        public OtherInfoPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
	        WaitForElementToBeVisible(_otherInfo);
	        if (!Driver.Url.EndsWith("other-information"))
	        {
		        throw new InvalidOperationException(
			        "This is not the other-information page, the current url is: " + Driver.Url);
	        }
        }

        public void TurnOffAudioRecording()
        {
	        // Waiting for clickable for radio button doesn't behave as expected so we don't use the base method here
	        Driver.FindElement(_recordAudioNo).Click();
        }
        
        public void EnterOtherInformation(string otherInformation)
		{
	        EnterText(_otherInfo, otherInformation);
		}

        public SummaryPage GoToNextPage()
        {
	        ClickElement(_nextButton);
	        return new SummaryPage(Driver, DefaultWaitTime);
        }
    }
}
