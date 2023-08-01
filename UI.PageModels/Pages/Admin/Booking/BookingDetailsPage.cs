using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using UI.PageModels.Dtos;

namespace UI.PageModels.Pages.Admin.Booking;

public class BookingDetailsPage : VhAdminWebPage
{
    private readonly By _closeBookingFailureWindowButton = By.Id("btnTryAgain");
    private readonly By _bookingConfirmedStatus = By.XPath("//div[@class='vh-created-booking'][text()='Confirmed']");
    private readonly By _cancelBookingButton = By.Id("cancel-button");
    private readonly By _editBookingButton = By.Id("edit-button");
    private readonly By _saveChangesButton = By.Id("bookButton");
    private readonly By _confirmCancelButton = By.Id("btnCancelBooking");
    private readonly By _cancelReason = By.Id("cancel-reason");
    private readonly By _participantDetails = By.ClassName("participant-details");
    private readonly By _courtRoomAddress = By.Id("court-room-address");
    private readonly By _hearingStart = By.Id("hearing-start");

    public BookingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeClickable(_editBookingButton);
        if (!Driver.Url.EndsWith("booking-details"))
            throw new InvalidOperationException(
                "This is not the booking-details page, the current url is: " + Driver.Url);
    }

    private By SpecificBookingConfirmedStatus(string caseNumber) => By.XPath(
        $"//div[@class='govuk-grid-column-full' and contains(.,'{caseNumber}') and contains(.,'Confirmed')]");

    private By SpecificBookingCancelledStatus(string caseNumber) => By.XPath(
        $"//div[@class='govuk-grid-column-full' and contains(.,'{caseNumber}') and contains(.,'Cancelled')]");

    /// <summary>
    /// Get the quick link join url from the booking details page. This is the url that can be used to join the hearing directly.
    /// </summary>
    /// <param name="videoWebUrl">The video web url to use as a fallback since remote drivers do not allow keyboard access.</param>
    /// <returns></returns>
    /// <exception cref="Exception">When join url is not found</exception>
    public string GetQuickLinkJoinUrl(string videoWebUrl)
    {
        var quickLinkJoinUrlLocator = By.Id("conference_join_by_link_details");
        WaitForElementToBeVisible(quickLinkJoinUrlLocator);
        ClickElement(quickLinkJoinUrlLocator);

        string quickLinkJoinUrl;

        // var script = "return sessionStorage.getItem('SelectedHearingIdKey')";
        if (Driver is IJavaScriptExecutor js)
        {
            var hearingId = GetHearingId();
            quickLinkJoinUrl = $"{videoWebUrl}/quickjoin/{hearingId}";
        }
        else
        {
            quickLinkJoinUrl = new TextCopy.Clipboard().GetText() ?? string.Empty;
        }

        if (!Uri.IsWellFormedUriString(quickLinkJoinUrl, UriKind.Absolute))
        {
            throw new Exception("The quick link join url is not a valid url: " + quickLinkJoinUrl);
        }

        return quickLinkJoinUrl;
    }

    /// <summary>
    /// Get a string value of the hearing id
    /// </summary>
    /// <returns></returns>
    public string GetHearingId()
    {
        const string script = "return sessionStorage.getItem('SelectedHearingIdKey')";
        return (string) (Driver as IJavaScriptExecutor)!.ExecuteScript(script) ?? string.Empty;
    }

    /// <summary>
    /// Get the string value of the allocated CSO
    /// </summary>
    /// <returns></returns>
    public string GetAllocatedTo()
    {
        return GetText(By.XPath("//div[@id='allocated-to']//strong"));
    }

    /// <summary>
    /// Edit a hearing and add participants to it
    /// </summary>
    /// <param name="participantsToAdd"></param>
    /// <returns></returns>
    public BookingConfirmationPage AddParticipantsToBooking(List<BookingExistingParticipantDto> participantsToAdd)
    {
        SwitchToEditMode();
        var participantsBreadcrumbLocator = By.XPath("//app-breadcrumb//div//ol//li//a[text()='Participants']");
        ClickElement(participantsBreadcrumbLocator);
        var participantsPage = new ParticipantsPage(Driver, DefaultWaitTime);
        participantsPage.AddExistingParticipants(participantsToAdd);
        return participantsPage.GoToVideoAccessPointsPage().GoToSummaryPage().ClickBookButton();
    }
    
    /// <summary>
    /// Edit a hearing and the scheduled date and duration
    /// </summary>
    /// <param name="newDate"></param>
    /// <param name="durationHour"></param>
    /// <param name="durationMinute"></param>
    /// <returns></returns>
    public BookingConfirmationPage UpdateSchedule(DateTime newDate, int durationHour, int durationMinute)
    {
        SwitchToEditMode();
        ClickElement(By.Id("edit-linkhearing-schedule-id"));
        var hearingSchedulePage = new HearingSchedulePage(Driver, DefaultWaitTime);
        hearingSchedulePage.EnterHearingDateAndDuration(newDate, durationHour, durationMinute);
        // in edit mode, next directs the user back to the summary page
        hearingSchedulePage.GoToNextPage();
        return new SummaryPage(Driver, DefaultWaitTime).ClickBookButton();
    }

    private void SwitchToEditMode()
    {
        ClickElement(_editBookingButton);
        WaitForElementToBeVisible(By.XPath("//main[@id='main-content']//app-summary//app-breadcrumb"));
    }
}