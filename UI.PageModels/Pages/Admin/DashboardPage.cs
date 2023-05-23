using OpenQA.Selenium;
using UI.PageModels.Pages.Admin.Booking;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin
{
    public class DashboardPage : VhPage
    {
	    private readonly By _bookHearingButton = By.Id("bookHearingBtn");
        
        public DashboardPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
	        WaitForApiSpinnerToDisappear();
	        WaitForElementToBeClickable(_bookHearingButton);
	        if (!Driver.Url.EndsWith("dashboard"))
	        {
		        throw new InvalidOperationException(
			        "This is not the dashboard page, the current url is: " + Driver.Url);
	        }
        }

        public HearingDetailsPage GoToBookANewHearing()
        {
	        ClickElement(_bookHearingButton);
	        return new HearingDetailsPage(Driver, DefaultWaitTime);
        }
    }
}
