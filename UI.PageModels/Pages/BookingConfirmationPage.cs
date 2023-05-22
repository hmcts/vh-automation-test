using OpenQA.Selenium;

namespace UI.PageModels.Pages
{
    public class BookingConfirmationPage : VhPage
    {
        private readonly By _viewBookingLink = By.XPath("//a[text()='View this booking']");
        private readonly By _bookAnotherHearingBtn = By.Id("btnBookAnotherHearing");

        public BookingConfirmationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            WaitForElementToBeClickable(_bookAnotherHearingBtn);
            if (!Driver.Url.EndsWith("booking-confirmation"))
            {
                throw new InvalidOperationException(
                    "This is not the booking-confirmation page, the current url is: " + Driver.Url);
            }
        }
        
        public void ClickViewBookingLink()
        {
            ClickElement(_viewBookingLink);
        }
    }
}
