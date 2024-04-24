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
    private readonly By _searchCaseTextBox = By.Id("caseNumber");
    private readonly By _searchButton = By.Id("searchButton");
    private readonly By _startDate = By.Id("startDate");
    private readonly By _endDate = By.Id("endDate");
    private readonly By _unallocatedOnly = By.XPath("//label[@for='noAllocated']");
    
    public BookingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
    }

    /// <summary>
    /// Open the 'Searc bookings' panel to filter for bookings
    /// </summary>
    /// <param name="queryDto"></param>
    public void SearchForBooking(BookingListQueryDto queryDto)
    {
        if(IsElementVisible(_searchPanelButton))
        {
            ClickElement(_searchPanelButton);
        }
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