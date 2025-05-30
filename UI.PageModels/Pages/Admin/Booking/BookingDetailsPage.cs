﻿using UI.Common.Utilities;

namespace UI.PageModels.Pages.Admin.Booking;

public class BookingDetailsPage : VhAdminWebPage
{
    private readonly By _editBookingButton = By.Id("edit-button");
    private readonly By _editThisAndUpcomingDaysButton = By.Id("edit-multiple-hearings-button");
    private readonly By _userNames = By.XPath("//div[contains(@id,'username')]");
    private readonly By _audioRecording = By.Id("audioRecorded");
    private readonly By _otherInformation = By.Id("otherInformation");
    private readonly By _cancelBookingButton = By.Id("cancel-button");
    private readonly By _cancelReasonDropdown = By.Id("cancel-reason");
    private readonly By _cancelSingleDayButton = By.Id("cancelSingleDayBooking");
    private readonly By _cancelThisAndUpcomingDaysButton = By.Id("btnCancelMultiDayBooking");
    private readonly By _cancelledStatus = By.XPath("(//div[@class='vh-status vh-cancelled-booking'])[1]");

    public BookingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
        if (IsBookingEditable())
        {
            WaitForElementToBeClickable(_editBookingButton);
        }
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
    public BookingConfirmationPage AddParticipantsToBooking(List<BookingParticipantDto> participantsToAdd)
    {
        SwitchToEditMode();
        var participantsBreadcrumbLocator = By.XPath("//app-breadcrumb//div//ol//li//a[text()='Participants']");
        ClickElement(participantsBreadcrumbLocator);
        var participantsPage = new ParticipantsPage(Driver, DefaultWaitTime, true);
        participantsPage.AddParticipants(participantsToAdd);
        var videoAccessPointsPage = participantsPage.GoToVideoAccessPointsPage();
        // use existing booking flow (i.e. next on vap goes screening (if enabled) and then to summary page)
        var summaryPage = FeatureToggle.Instance().SpecialMeasuresEnabled() ?
            videoAccessPointsPage.GoToSpecialMeasuresPage().GoToOtherInformationPage().GoToSummaryPage()
            : videoAccessPointsPage.GoToOtherInformationPage().GoToSummaryPage();
        return summaryPage.ClickBookButton();
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
                        $"//div[normalize-space()='{endpoint.DisplayName}']/../child::div[contains(normalize-space(),'{endpoint.DefenceAdvocateDisplayName}')]//fa-icon");

                if (!IsElementVisible(linkToAdvocate))
                {
                    throw new InvalidOperationException($"Link icon between VAP {endpoint.DisplayName} and {endpoint.DefenceAdvocateDisplayName} is not visible");
                }
            }
            if (endpoint.InterpreterLanguage != null)
            {
                CompareText(By.XPath($"//div[normalize-space()='{endpoint.InterpreterLanguage.Description}']"), endpoint.InterpreterLanguage.Description);
            }
        }
        ValidateParticipants(bookingDto.Participants.Concat(bookingDto.NewParticipants).ToList());
        ValidateDetails(bookingDto);
        ValidateScreeningDetails(bookingDto);
    }

    public void ValidateBookingIsCancelled()
    {
        if (!IsElementVisible(_cancelledStatus))
        {
            throw new InvalidOperationException("Cancelled status is not visible");
        }
    }

    public void ClickCancelBooking()
    {
        ClickElement(_cancelBookingButton);
    }

    public void CancelSingleHearing(string cancellationReason)
    {
        SetCancellationReason(cancellationReason);
        ClickElement(_cancelSingleDayButton);
        WaitForApiSpinnerToDisappear();
    }

    public void CancelThisAndUpcomingDays(string cancellationReason)
    {
        SetCancellationReason(cancellationReason);
        ClickElement(_cancelThisAndUpcomingDaysButton);
        WaitForApiSpinnerToDisappear();
    }
    
    private void SetCancellationReason(string cancellationReason)
    {
        WaitForDropdownListToPopulate(_cancelReasonDropdown);
        SelectDropDownByText(_cancelReasonDropdown, cancellationReason);
    }

    private void ValidateParticipants(List<BookingParticipantDto> participants)
    {
        var userNames = ExtractUserNames().ToArray();
        foreach (var participant in participants)
        {
            if(!userNames.Contains(participant.Username, StringComparer.InvariantCultureIgnoreCase))
                throw new InvalidOperationException($"Expected participant username: {participant.Username} but was not found on the page.");
            if (participant.InterpreterLanguage != null)
            {
                CompareText(By.XPath($"//div[normalize-space()='{participant.InterpreterLanguage.Description}']"), participant.InterpreterLanguage.Description);
            }
        }
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

    private bool IsBookingEditable()
    {
        return !IsElementVisible(_cancelledStatus);
    }

    /// <summary>
    /// Get the SIP connection details at a specific position
    /// </summary>
    /// <param name="position">Greater than zero</param>
    /// <returns></returns>
    public (string, string) GetSipConnectionDetailsAtPosition(int position)
    {
        ArgumentOutOfRangeException.ThrowIfZero(position);
        var locator = By.XPath($"(//div[contains(text(), '{SipAddressStem}')])[{position}]");
        var text = GetText(locator);
        var split = text.Split(":");
        var address = split[0].Trim();
        var pin = split[1].Trim();
        
        return (address, pin);
    }

    /// <summary>
    /// Edit the hearing and go to the special measures page
    /// </summary>
    /// <returns></returns>
    public SpecialMeasuresPage EditSpecialMeasures()
    {
        ClickElement(_editBookingButton);
        WaitForElementToBeVisible(By.XPath("//main[@id='main-content']//app-summary//app-breadcrumb"));
        var specialMeasuresBreadcrumbLocator = By.XPath("//app-breadcrumb//div//ol//li//a[text()='Screening (Special Measure)']");
        ClickElement(specialMeasuresBreadcrumbLocator);
        var specialMeasuresPage = new SpecialMeasuresPage(Driver, DefaultWaitTime);
        return specialMeasuresPage;
    }
}