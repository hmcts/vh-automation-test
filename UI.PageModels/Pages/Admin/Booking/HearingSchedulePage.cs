﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace UI.PageModels.Pages.Admin.Booking;

public class HearingSchedulePage : VhAdminWebPage
{
    private readonly By _courtRoom = By.Id("court-room");
    private readonly By _courtVenue = By.Id("courtAddress");
    private readonly By _hearingDate = By.Id("hearingDate");
    private readonly By _endOfHearingDate = By.Id("endHearingDate");
    private readonly By _hearingDurationHour = By.Id("hearingDurationHour");
    private readonly By _hearingDurationMinute = By.Id("hearingDurationMinute");
    private readonly By _hearingStartTimeHour = By.Id("hearingStartTimeHour");
    private readonly By _hearingStartTimeMinute = By.Id("hearingStartTimeMinute");
    private readonly By _multiDaysHearingCheckbox = By.XPath("//input[@id='multiDaysHearing' and @type='checkbox']");
    private readonly By _nextButton = By.Id("nextButton");
    
    private readonly IWebDriver driver;
    private readonly int defaultWaitTime;

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
    
    public void EnterHearingDateAndRange(DateTime date, DateTime endDateTime, int durationHour, int durationMinute)
    {
        var dateString = GetLocaleDate(date);
        var endDateString = GetLocaleDate(endDateTime);
        EnterText(_hearingDate, dateString);
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