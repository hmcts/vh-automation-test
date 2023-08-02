namespace UI.PageModels.Pages.Admin.Booking;

public class BookingConfirmationPage : VhAdminWebPage
{
    private readonly By _bookAnotherHearingBtn = By.Id("btnBookAnotherHearing");
    private readonly By _viewBookingLink = By.XPath("//a[text()='View this booking']");

    public BookingConfirmationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_bookAnotherHearingBtn);
        if (!Driver.Url.EndsWith("booking-confirmation"))
            throw new InvalidOperationException(
                "This is not the booking-confirmation page, the current url is: " + Driver.Url);
    }

    public string GetNewHearingId()
    {
        const string script = "return sessionStorage.getItem('newHearingId')";
        return (string) (Driver as IJavaScriptExecutor)!.ExecuteScript(script) ?? string.Empty;
    }
    
    public bool IsBookingSuccessful()
    {
        return IsElementVisible(By.XPath("//h1[normalize-space()='Your hearing booking was successful']"));
    }

    public BookingDetailsPage ClickViewBookingLink()
    {
        ClickElement(_viewBookingLink);
        return new BookingDetailsPage(Driver, DefaultWaitTime);
    }
}