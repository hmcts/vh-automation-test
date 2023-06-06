using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.WorkAllocation;

public class EditHoursSection : VhAdminWebPage
{
    private readonly By _editHoursSectionBtn = By.Id("edit-availability");
    private readonly By _searchForUserBtn = By.Id("vho-search-form-btn");
    
    private readonly By _usernameSearchField = By.Id("username");
    private readonly By _editWorkingHoursRadioBtn = By.XPath("//label[normalize-space()='Edit working hours']");
    private readonly By _editWorkingHoursBtn = By.Id("edit-individual-work-hours-button");
    private readonly By _saveWorkHoursBtn = By.Id("save-individual-work-hours-button");
    private readonly By _cancelWorkHoursBtn = By.Id("save-individual-work-hours-button");
    private readonly By _editWorkHoursSavedSuccess = By.Id("edit-upload-hours-success");
    
    private readonly By _editNonAvailabilityHoursRadioButton = By.XPath("//label[normalize-space()='Edit non-availability hours']");
    private readonly By _addNewNonAvailabilityBtn = By.Id("non-available-table-add-new-btn");
    private readonly By _editNonAvailableHoursDateFilter = By.Id("start-date");
    private readonly By _editNonAvailableHoursDateFilterBtn = By.Id("filter-btn");
    private readonly By _saveNonAvailableBtn = By.Id("save-individual-non-work-hours-button");
    private readonly By _editNonAvailableHoursSavedSuccess = By.Id("edit-non-availability-successful-message-container");

    public EditHoursSection(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public void EditWorkingHours(string username, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        CheckSectionIsOpen();

        ClickElement(_editWorkingHoursRadioBtn);
        EnterText(_usernameSearchField, username);
        ClickElement(_searchForUserBtn);
        ClickElement(_editWorkingHoursBtn);

        var row = GetEditHoursRowByDay(dayOfWeek);
        var startTimeTextBox = row.FindElement(By.XPath("td[2]/input"));
        var endTimeTextBox = row.FindElement(By.XPath("td[3]/input"));

        startTimeTextBox.SendKeys(startTime.ToString("HHmm"));
        endTimeTextBox.SendKeys(endTime.ToString("HHmm"));
        row.Click();

        ClickElement(_saveWorkHoursBtn);
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_editWorkHoursSavedSuccess);
    }

    public void FilterForNonAvailableDays(string username, DateTime startDateTime)
    {
        CheckSectionIsOpen();
        
        EnterUserToEditNonAvailableHours(username);
        var dateString = GetLocaleDate(startDateTime);
        EnterText(_editNonAvailableHoursDateFilter, dateString);
        ClickElement(_editNonAvailableHoursDateFilterBtn);
    }

    public void AddNonAvailableDayForUser(string username, DateTime startDateTime, DateTime endDateTime, bool deletePostSave = true)
    {
        CheckSectionIsOpen();
        
        EnterUserToEditNonAvailableHours(username);

        ClickElement(_addNewNonAvailabilityBtn);
        var row = GetNewAddedNonAvailabilityRow();

        var startDateString = GetLocaleDate(startDateTime);
        var endDateString = GetLocaleDate(startDateTime);
        
        var startDateTextBox = row.FindElement(By.XPath("td[1]/input"));
        var endDateTextBox = row.FindElement(By.XPath("td[2]/input"));
        var startTimeTextBox = row.FindElement(By.XPath("td[3]/input"));
        var endTimeTextBox = row.FindElement(By.XPath("td[4]/input"));
        
        endDateTextBox.SendKeys(endDateString);
        startDateTextBox.SendKeys(startDateString);
        startTimeTextBox.SendKeys(startDateTime.ToString("HHmm"));
        endTimeTextBox.SendKeys(endDateTime.ToString("HHmm"));
        
        ClickElement(_saveNonAvailableBtn);
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_editNonAvailableHoursSavedSuccess);
        
        if(!deletePostSave) return;
        
        // need to update the row else you will get a stale element exception
        row = GetNewAddedNonAvailabilityRow();
        var deleteRowButton = row.FindElement(By.XPath("td[5]/fa-icon"));
        deleteRowButton.Click();
        WaitForElementToBeVisible(By.TagName("app-confirm-delete-hours-popup"));
        ClickElement(By.Id("btnConfirm"));
        WaitForApiSpinnerToDisappear();
    }

    private void EnterUserToEditNonAvailableHours(string username)
    {
        ClickElement(_editNonAvailabilityHoursRadioButton);
        EnterText(_usernameSearchField, username);
        ClickElement(_searchForUserBtn);
    }

    private IWebElement GetNewAddedNonAvailabilityRow()
    {
        var locator = By.XPath($"//table[@id='non-available-table-results']/tbody/tr[last()]");
        var elem =  Driver.FindElement(locator);
        return elem;
    }

    private IWebElement GetEditHoursRowByDay(DayOfWeek day)
    {
        var dayAsString = day.ToString();
        var locator = By.XPath($"//td[normalize-space()='{dayAsString}']/..");
        return Driver.FindElement(locator);
    }
    
    private void CheckSectionIsOpen()
    {
        if (IsElementVisible(_searchForUserBtn)) return;
        ClickElement(_editHoursSectionBtn);
    }
}