using OpenQA.Selenium;
using UI.PageModels.Pages.Admin.Booking;

namespace UI.PageModels.Pages.Admin;

/// <summary>
///     BookingListPage
///     Page element definitions
///     Do not add logic here
/// </summary>
public class BookingListPage : VhAdminWebPage
{
    private readonly By _searchPanelButton = By.Id("openSearchPanelButton");
    private By _searchCaseTextBox = By.Id("caseNumber");
    private readonly By _videoHearingsTable = By.Id("vh-table");
    private readonly By _hearingDateTitle = By.XPath($"//div[text()[contains(.,'{DateTime.Today.ToString("dd MMMM yyyy")}')]]");
    private readonly By _hearingDetailsRow = By.XPath("//div[@class='vh-row-created']//div[@class='govuk-grid-row vh-row vh-a']");
    private readonly By _searchButton = By.Id("searchButton");
    private readonly By _confirmedButton = By.XPath("//*[contains(text(),'Confirmed')]");
    private readonly By _telephoneParticipantLink = By.XPath("//div[@id='conference_phone_details']");
    private readonly By _videoParticipantLink = By.XPath("//div[contains(text(),'video-participant-link')]");
    
    private readonly By _venueListbox = By.Id("venues");
    private readonly By _allHearings = By.XPath("//div[@class='govuk-grid-row vh-row vh-a']");
    private readonly By _startDate = By.Id("startDate");
    private readonly By _endDate = By.Id("endDate");
    private readonly By _unallocatedOnly = By.XPath("//label[@for='noAllocated']");
    private readonly By _caseTypes = By.Id("caseTypes");

    public BookingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
    }

    // private readonly By HearingDetailsRowSpecific(string caseNumber) = By.XPath($"//div[text()[contains(.,'{caseNumber}')]]");
    //
    // private By HearingSelectionSpecificRow(string caseNumber) = By.XPath(
    //     $"//div[@class='govuk-grid-row vh-row vh-a' and contains(.,'{caseNumber}')]//div[@class='vh-created-booking']");
    //
    // private By VenueCheckbox(string venue) = By.XPath($"//input[@aria-label='Venue name {venue}']");
    // private By CaseTypeCheckbox(string caseType) = By.XPath($"//input[@aria-label='Case type {caseType}']");

    /// <summary>
    /// Open the 'Searc bookings' panel to filter for bookings
    /// </summary>
    /// <param name="queryDto"></param>
    public void SearchForBooking(BookingListQueryDto queryDto)
    {
        IsElementVisible(_searchPanelButton);
        ClickElement(_searchPanelButton);
        if (!string.IsNullOrWhiteSpace(queryDto.CaseNumber))
        {
            EnterText(_searchCaseTextBox, queryDto.CaseNumber);
        }
        
        if(queryDto.StartDate.HasValue)
        {
            var dateString = GetLocaleDate(queryDto.StartDate.Value);
            EnterText(_startDate, dateString);
        }
        
        if(queryDto.StartDate.HasValue && queryDto.EndDate.HasValue)
        {
            var dateString = GetLocaleDate(queryDto.EndDate.Value);
            EnterText(_endDate, dateString);
        }

        if (queryDto.UnallocatedOnly)
        {
            ClickElement(_unallocatedOnly);
        }
        
        ClickElement(_searchButton);
        WaitForApiSpinnerToDisappear();
    }

    /// <summary>
    /// Select a hearing to view by case number
    /// </summary>
    /// <param name="caseNumber"></param>
    /// <returns>BookingDetails page for the selected hearing</returns>
    public BookingDetailsPage ViewBookingDetails(string caseNumber)
    {
        ClickElement(By.XPath($"(//div[contains(text(),'{caseNumber}')])[1]"));
        WaitForApiSpinnerToDisappear();
        return new BookingDetailsPage(Driver, DefaultWaitTime);
    }
}

public class BookingListQueryDto
{
    public string CaseNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool UnallocatedOnly { get; set; } = false;
}