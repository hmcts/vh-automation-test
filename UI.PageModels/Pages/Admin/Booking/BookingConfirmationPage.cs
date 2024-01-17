using OpenQA.Selenium.Support.UI;

namespace UI.PageModels.Pages.Admin.Booking;

public class BookingConfirmationPage : VhAdminWebPage
{
    private readonly By _bookAnotherHearingBtn = By.Id("btnBookAnotherHearing");
    private readonly By _viewBookingLink = By.XPath("//a[text()='View this booking']");

    private readonly By _viewThisBooking =
        By.XPath("//*[@id='main-content']/app-booking-confirmation/div/div/div[1]/a");

    private readonly By _editHearing = By.XPath("//button[@id='edit-button']");
    private readonly By _addNewNOJOH = By.LinkText("Judicial Office Holder(s)");
    private readonly By _nextParticipantPage = By.XPath("//*[@id='nextButtonToParticipants']");

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

    public void viewThisBooking()
    {
        ClickElement(_viewThisBooking);
    }

    public void editHearing()
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
        js.ExecuteScript("arguments[0].scrollIntoView(true);", 
            Driver.FindElement(_editHearing));
        ClickElement(_editHearing);
    }

    public void AddNewNoJoh()
    {
        ClickElement(_addNewNOJOH);
    }

    public void NextParticipantPage()
    {
        ClickElement(_nextParticipantPage);
    }
}