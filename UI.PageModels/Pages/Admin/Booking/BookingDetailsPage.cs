using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace UI.PageModels.Pages.Admin.Booking;

public class BookingDetailsPage : VhAdminWebPage
{
    public static By CloseBookingFailureWindowButton = By.Id("btnTryAgain");
    public static By BookingConfirmedStatus = By.XPath("//div[@class='vh-created-booking'][text()='Confirmed']");

    public BookingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(EditBookingButton);
        if (!Driver.Url.EndsWith("booking-details"))
            throw new InvalidOperationException(
                "This is not the booking-details page, the current url is: " + Driver.Url);
    }

    public static By CancelBookingButton => By.Id("cancel-button");
    public static By EditBookingButton => By.Id("edit-button");

    public static By ConfirmCancelButton => By.Id("btnCancelBooking");
    public static By CancelReason => By.Id("cancel-reason");
    public static By ParticipantDetails => By.ClassName("participant-details");
    public static By CourtRoomAddress => By.Id("court-room-address");
    public static By HearingStart => By.Id("hearing-start");

    public static By SpecificBookingConfirmedStatus(string caseNumber) => By.XPath(
        $"//div[@class='govuk-grid-column-full' and contains(.,'{caseNumber}') and contains(.,'Confirmed')]");

    public static By SpecificBookingCancelledStatus(string caseNumber) => By.XPath(
        $"//div[@class='govuk-grid-column-full' and contains(.,'{caseNumber}') and contains(.,'Cancelled')]");

    public string GetQuickLinkJoinUrl(string videoWebUrl)
    {
        var quickLinkJoinUrlLocator = By.Id("conference_join_by_link_details");
        WaitForElementToBeVisible(quickLinkJoinUrlLocator);
        ClickElement(quickLinkJoinUrlLocator);
        
        string quickjoinUrl = null;

        var script = "return sessionStorage.getItem('SelectedHearingIdKey')";
        if (Driver is IJavaScriptExecutor js)
        {
            var hearingId = (string) js.ExecuteScript(script);
            quickjoinUrl = $"{videoWebUrl}/quickjoin/{hearingId}";
        }
        // else if (Driver is ChromeDriver cd)
        // {
        //     var hearingId = (string) cd.ExecuteScript(script);
        //     quickjoinUrl = $"{videoWebUrl}/quickjoin/{hearingId}";
        // }
        // else if (Driver is RemoteWebDriver rwd)
        // {
        //     var hearingId = (string) rwd.ExecuteScript(script);
        //     quickjoinUrl = $"{videoWebUrl}/quickjoin/{hearingId}";
        // }
        else
        {
            quickjoinUrl = new TextCopy.Clipboard().GetText() ?? string.Empty;
        }
        if (!Uri.IsWellFormedUriString(quickjoinUrl, UriKind.Absolute))
        {
            throw new Exception("The quick link join url is not a valid url: " + quickjoinUrl);
        }
        return quickjoinUrl;
    }

    public string GetAllocatedTo()
    {
        return GetText(By.XPath("//div[@id='allocated-to']//strong"));
    }
}