namespace UI.PageModels.Pages.Admin.Booking;

public class BookingDetailsPage : VhAdminWebPage
{
    private readonly By _editBookingButton = By.Id("edit-button");
    private readonly By _editThisAndUpcomingDaysButton = By.Id("edit-multiple-hearings-button");
    private readonly By _userNames = By.XPath("//div[contains(@id,'username')]");
    private readonly By _audioRecording = By.Id("audioRecorded");
    private readonly By _otherInformation = By.Id("otherInformation");
    
    public BookingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeClickable(_editBookingButton);
        if (!Driver.Url.EndsWith("booking-details"))
            throw new InvalidOperationException(
                "This is not the booking-details page, the current url is: " + Driver.Url);
    }

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
            throw new InvalidOperationException("The quick link join url is not a valid url: " + quickLinkJoinUrl);
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
    public BookingConfirmationPage AddParticipantsToBooking(List<BookingParticipantDto> participantsToAdd, bool useParty)
    {
        SwitchToEditMode();
        var participantsBreadcrumbLocator = By.XPath("//app-breadcrumb//div//ol//li//a[text()='Participants']");
        ClickElement(participantsBreadcrumbLocator);
        var participantsPage = new ParticipantsPage(Driver, DefaultWaitTime, useParty);
        participantsPage.AddParticipants(participantsToAdd);
        return participantsPage.GoToVideoAccessPointsPage().GoToSummaryPage().ClickBookButton();
    }
    
    /// <summary>
    /// Edit a hearing and the scheduled date and duration
    /// </summary>
    /// <param name="newDate"></param>
    /// <param name="durationHour"></param>
    /// <param name="durationMinute"></param>
    /// <returns></returns>
    public SummaryPage UpdateSchedule(DateTime newDate, int durationHour, int durationMinute)
    {
        SwitchToEditMode();
        ClickElement(By.Id("edit-linkhearing-schedule-id"));
        var hearingSchedulePage = new HearingSchedulePage(Driver, DefaultWaitTime);
        hearingSchedulePage.EnterHearingDateAndDuration(newDate, durationHour, durationMinute);
        // in edit mode, next directs the user back to the summary page
        hearingSchedulePage.GoToNextPage();
        return new SummaryPage(Driver, DefaultWaitTime);
    }

    public SummaryPage UpdateScheduleForMultipleHearings(List<DateTime> newDates, int startTimeHour, int endTimeHour, int durationHour, int durationMinute)
    {
        EditThisAndUpcomingDays();
        ClickElement(By.Id("edit-linkhearing-schedule-id"));
        var hearingSchedulePage = new HearingSchedulePage(Driver, DefaultWaitTime);
        hearingSchedulePage.EnterNewDatesAndDuration(newDates, startTimeHour, endTimeHour, durationHour, durationMinute);
        hearingSchedulePage.GoToNextPage();
        return new SummaryPage(Driver, DefaultWaitTime);
    }

    /// <summary>
    /// Validate the confirmation page matches the details provided by the booking dto
    /// </summary>
    /// <param name="bookingDto"></param>
    /// <exception cref="Exception">When an assertion fails</exception>
    public void ValidateDetailsPage(BookingDto bookingDto)
    {
        for (var i = 0; i < bookingDto.VideoAccessPoints.Count-1; i++)
        {
            var endpoint = bookingDto.VideoAccessPoints[i];
            CompareText(By.XPath($"//div[normalize-space()='{endpoint.DisplayName}']"), endpoint.DisplayName);
            if (!string.IsNullOrWhiteSpace(endpoint.DefenceAdvocateDisplayName))
            {
                var linkToAdvocate =
                    By.XPath(
                        $"//div[normalize-space()='{endpoint.DisplayName}']/../child::div[contains(normalize-space(),'{endpoint.DefenceAdvocateDisplayName}')]//img[@alt='link to endpoint']");

                if (!IsElementVisible(linkToAdvocate))
                {
                    throw new InvalidOperationException($"Link icon between VAP {endpoint.DisplayName} and {endpoint.DefenceAdvocateDisplayName} is not visible");
                }
            }
        }
        ValidateParticipants(bookingDto.Participants.Concat(bookingDto.NewParticipants).ToList());
        ValidateDetails(bookingDto);
    }

    private void ValidateParticipants(List<BookingParticipantDto> participants)
    {
        var userNames = ExtractUserNames().ToArray();
        foreach (var participant in participants)
            if(!userNames.Contains(participant.Username, StringComparer.InvariantCultureIgnoreCase))
                throw new InvalidOperationException($"Expected participant username: {participant.Username} but was not found on the page.");
    }

    private void ValidateDetails(BookingDto bookingDto)
    {
        CompareText(_audioRecording, BoolToString(bookingDto.AudioRecording));
        CompareText(_otherInformation, bookingDto.OtherInformation);
    }

    private void CompareText(By element, string expectedText)
    {
        var text = GetText(element);
        if (!text.Equals(expectedText, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidOperationException($"Expected text: {expectedText} but was {text}");
        }
    }
    
    private void SwitchToEditMode()
    {
        ClickElement(_editBookingButton);
        WaitForElementToBeVisible(By.XPath("//main[@id='main-content']//app-summary//app-breadcrumb"));
    }

    private void EditThisAndUpcomingDays()
    {
        ClickElement(_editThisAndUpcomingDaysButton);
        WaitForElementToBeVisible(By.XPath("//main[@id='main-content']//app-summary//app-breadcrumb"));
    }
    
    private IEnumerable<string> ExtractUserNames()
    {
        var userNamesWebElements = Driver.FindElements(_userNames);
        foreach (var webElement in userNamesWebElements)
            yield return webElement.Text;
    }

    private static string BoolToString(bool boolean) => boolean ? "Yes" : "No";
}