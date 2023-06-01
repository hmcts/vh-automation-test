using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.Booking
{
    public class HearingAssignJudgePage : VhPage
    {
        private readonly By _judgeEmail = By.Id("judge-email");
        private readonly By _judgeDisplayNameFld = By.Id("judgeDisplayNameFld");
        private readonly By _judgePhoneFld = By.Id("judgePhoneFld");
        private readonly By _nextButton = By.Id("nextButton");
        private readonly By _searchResults = By.Id("search-results-list");

        public HearingAssignJudgePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
	        WaitForApiSpinnerToDisappear();
        }

        public void EnterJudgeDetails(string judgeEmail, string judgeDisplayName, string judgePhone)
        {
	        EnterText(_judgeEmail, judgeEmail);
	        WaitForElementToBeVisible(Spinner); // wait 2 seconds before search starts
	        WaitForApiSpinnerToDisappear();
	        WaitForElementToBeVisible(_searchResults);
	        ClickElement(_searchResults);
	        if (!string.IsNullOrWhiteSpace(judgeDisplayName))
	        {
		        EnterText(_judgeDisplayNameFld, judgeDisplayName);
	        }
	        if (!string.IsNullOrWhiteSpace(judgePhone))
	        {
		        EnterText(_judgePhoneFld, judgePhone);
	        }
        }
        
        public ParticipantsPage GoToNextPage()
		{
	        ClickElement(_nextButton);
	        return new ParticipantsPage(Driver, DefaultWaitTime);
		}
    }
}
