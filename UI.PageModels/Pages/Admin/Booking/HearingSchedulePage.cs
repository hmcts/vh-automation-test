using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin.Booking;

public class HearingSchedulePage : VhAdminWebPage
{
    private readonly By _courtRoom = By.Id("court-room");
    private readonly By _courtVenue = By.Id("courtAddress");
    private readonly By _hearingDate = By.Id("hearingDate");
    private readonly By _hearingDurationHour = By.Id("hearingDurationHour");
    private readonly By _hearingDurationMinute = By.Id("hearingDurationMinute");
    private readonly By _hearingStartTimeHour = By.Id("hearingStartTimeHour");
    private readonly By _hearingStartTimeMinute = By.Id("hearingStartTimeMinute");
    private readonly By _multiDaysHearing = By.Id("multiDaysHearing");
    private readonly By _nextButton = By.Id("nextButton");

    public HearingSchedulePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
        WaitForDropdownListToPopulate(_courtVenue);
    }

    public void EnterSingleDayHearingSchedule(DateTime date, int durationHour, int durationMinute, string venueName,
        string roomName)
    {
        EnterHearingDateAndDuration(date, durationHour, durationMinute);
        EnterHearingVenueAndRoom(venueName, roomName);
    }

    public HearingAssignJudgePage GoToNextPage()
    {
        var errors = Driver.FindElements(By.ClassName("govuk-error-message")).Select(x => x.Text);
        var message = string.Join("; ", errors);

        if (HasFormValidationError())
        {
            throw new InvalidOperationException($"Form has validation errors.", new InvalidOperationException(message));
        }

        ClickElement(_nextButton);
        return new HearingAssignJudgePage(Driver, DefaultWaitTime);
    }

    public void EnterHearingDateAndDuration(DateTime date, int durationHour, int durationMinute)
    {
        var dateString = GetLocaleDate(date);
        EnterText(_hearingDate, dateString);
        EnterText(_hearingStartTimeHour, date.ToString("HH"));
        EnterText(_hearingStartTimeMinute, date.ToString("mm"));
        EnterText(_hearingDurationHour, durationHour.ToString());
        EnterText(_hearingDurationMinute, durationMinute.ToString());
    }

    private void EnterHearingVenueAndRoom(string venueName, string roomName)
    {
        SelectDropDownByText(_courtVenue, venueName);
        EnterText(_courtRoom, roomName);
    }
}