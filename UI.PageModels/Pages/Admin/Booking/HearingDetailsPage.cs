using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.Booking
{
    public class HearingDetailsPage : VhPage
    {
        private readonly By _caseNumber = By.Id("caseNumber");
        private readonly By _caseName = By.Id("caseName");
        private readonly By _caseType = By.Id("caseType");
        private readonly By _hearingType = By.Id("hearingType");
        private readonly By _nextButton = By.Id("nextButton");

        public HearingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
	        if (!Driver.Url.EndsWith("book-hearing"))
	        {
		        throw new InvalidOperationException(
			        "This is not the HearingDetails page, the current url is: " + Driver.Url);
	        }
        }
        
        public void EnterHearingDetails(string caseNumber, string caseName, string caseType, string hearingType)
		{
			WaitForApiSpinnerToDisappear();
			EnterText(_caseName, caseName);
			EnterText(_caseNumber, caseNumber);
			WaitForDropdownListToPopulate(_caseType);
			SelectDropDownByText(_caseType, caseType);
			SelectDropDownByText(_hearingType, hearingType);
		}
        
        public HearingSchedulePage GoToNextPage()
		{
			ClickElement(_nextButton);
			return new HearingSchedulePage(Driver, DefaultWaitTime);
		}
    }
}
