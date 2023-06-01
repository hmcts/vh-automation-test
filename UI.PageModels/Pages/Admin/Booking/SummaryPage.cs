using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.Booking
{
    public class SummaryPage : VhPage
    {
        private readonly By _bookButton = By.Id("bookButton");
        private readonly By _dotLoader = By.Id("dot-loader");
        private readonly By _tryAgainButton = By.Id("btnTryAgain");
        //private readonly By SuccessTitle = By.ClassName("govuk-panel__title");
        private readonly By _successTitle = By.XPath("//h1[text()[contains(.,'Your hearing booking was successful')]]");

        public static By ViewThisBooking = By.LinkText("View this booking");

        public SummaryPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            WaitForElementToBeClickable(_bookButton);
            if (!Driver.Url.EndsWith("summary"))
            {
                throw new InvalidOperationException(
                    "This is not the summary page, the current url is: " + Driver.Url);
            }
        }
        
        public BookingConfirmationPage ClickBookButton()
        {
            ClickElement(_bookButton);
            WaitForElementToBeVisible(Spinner);
            WaitForApiSpinnerToDisappear(90); // booking process can take a while. lower when process has been optimised
            WaitForElementToBeVisible(_successTitle);
            return new BookingConfirmationPage(Driver, DefaultWaitTime);
        }
    }
}
