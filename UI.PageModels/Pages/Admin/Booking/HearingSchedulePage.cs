using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace UI.PageModels.Pages.Admin.Booking;

public class HearingSchedulePage : VhAdminWebPage
{
    private readonly By _courtRoom = By.Id("court-room");
    private readonly By _courtVenue = By.Id("courtAddress");
    private readonly By _hearingDate = By.Id("hearingDate");
    private readonly By _multiIndividualHearingDate = By.Id("multiHearingDateIndividual");
    private readonly By _startOfHearingDate = By.Id("startHearingDate");
    private readonly By _endOfHearingDate = By.Id("endHearingDate");
    private readonly By _hearingDurationHour = By.Id("hearingDurationHour");
    private readonly By _hearingDurationMinute = By.Id("hearingDurationMinute");
    private readonly By _hearingStartTimeHour = By.Id("hearingStartTimeHour");
    private readonly By _hearingStartTimeMinute = By.Id("hearingStartTimeMinute");
    private readonly By _multiDaysHearingCheckbox = By.XPath("//input[@id='multiDaysHearing' and @type='checkbox']");
    private readonly By _nextButton = By.Id("nextButton");
    private readonly By _selectIndividualDates = By.Id("multiDaysRange-choice-yes-label");
    private readonly By _addHearingDateButton = By.XPath("//button[text()='Add hearing date']");

    public HearingSchedulePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
        WaitForDropdownListToPopulate(_courtVenue);
    }

    public void EnterSingleDayHearingSchedule(BookingDto bookingDto)
    {
        EnterHearingDateAndDuration(bookingDto.ScheduledDateTime, bookingDto.DurationHour, bookingDto.DurationMinute);
        EnterHearingVenueAndRoom(bookingDto.VenueName, bookingDto.RoomName);
    }   
    
    public void EnterMultiDayHearingSchedule(BookingDto bookingDto)
    {
        ClickMultiDaysHearingCheckbox();
        EnterHearingDateAndRange(bookingDto.ScheduledDateTime, bookingDto.EndDateTime, bookingDto.DurationHour, bookingDto.DurationMinute);
        EnterHearingVenueAndRoom(bookingDto.VenueName, bookingDto.RoomName);
    }
    
    public void EnterMultiDayHearingScheduleWithIndividualDates(BookingDto bookingDto, out List<DateTime>? individualDatesForValidation)
    {
        ClickMultiDaysHearingCheckbox();
        ClickElement(_selectIndividualDates);
        var i = 0;
        DateTime bookingDate;
        individualDatesForValidation = new List<DateTime>();
        do
        {
            bookingDate = new DateTime(bookingDto.ScheduledDateTime.Ticks, DateTimeKind.Utc).AddDays(i++);
            individualDatesForValidation.Add(bookingDate);
            EnterHearingDateIndividualDates(bookingDate);
        } while (bookingDate < bookingDto.EndDateTime);
        
        EnterText(_hearingStartTimeHour, bookingDate.ToString("HH"));
        EnterText(_hearingStartTimeMinute, bookingDate.ToString("mm"));
        EnterText(_hearingDurationHour, bookingDto.DurationHour.ToString());
        EnterText(_hearingDurationMinute, bookingDto.DurationMinute.ToString());
        
        EnterHearingVenueAndRoom(bookingDto.VenueName, bookingDto.RoomName);
    }

    public void EnterHearingDateIndividualDates(DateTime bookingDate)
    {
        var dateString = GetLocaleDate(bookingDate);
        ClickElement(_addHearingDateButton);
        EnterText(_multiIndividualHearingDate, dateString);
    }
    
    public void EnterHearingDateAndRange(DateTime date, DateTime endDateTime, int durationHour, int durationMinute)
    {
        var dateString = GetLocaleDate(date);
        var endDateString = GetLocaleDate(endDateTime);
        EnterText(_startOfHearingDate, dateString);
        EnterText(_endOfHearingDate,endDateString);
        EnterText(_hearingStartTimeHour, date.ToString("HH"));
        EnterText(_hearingStartTimeMinute, date.ToString("mm"));
        EnterText(_hearingDurationHour, durationHour.ToString());
        EnterText(_hearingDurationMinute, durationMinute.ToString());
    }

    public HearingAssignJudgePage GoToNextPage()
    {
        if (HasFormValidationError())
        {
            var message = GetValidationErrors();
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

    public void EnterNewDatesAndDuration(List<DateTime> newDates, int startTimeHour, int endTimeHour, int durationHour, int durationMinute)
    {
        var newDateIndex = 1;
        
        foreach (var newDate in newDates)
        {
            var dateString = GetLocaleDate(newDate);
            var newDateInput = GetNewDateInput(newDateIndex);
            EnterText(newDateInput, dateString);

            newDateIndex++;
        }
        
        EnterText(_hearingStartTimeHour, startTimeHour.ToString("00"));
        EnterText(_hearingStartTimeMinute, endTimeHour.ToString("00"));
        EnterText(_hearingDurationHour, durationHour.ToString());
        EnterText(_hearingDurationMinute, durationMinute.ToString());
    }
    
    private static By GetNewDateInput(int index) => 
        By.XPath($"(//input[@type='date'])[{index}]");

    private void EnterHearingVenueAndRoom(string venueName, string roomName)
    {
        SelectDropDownByText(_courtVenue, venueName);
        EnterText(_courtRoom, roomName);
    }
    

    private void ClickMultiDaysHearingCheckbox()
    {
        WaitForElementVisible(Driver, _multiDaysHearingCheckbox);
        Driver.FindElement(_multiDaysHearingCheckbox).Click();
    }
    
}