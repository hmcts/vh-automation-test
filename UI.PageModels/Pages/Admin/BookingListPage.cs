using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin;

/// <summary>
///     BookingListPage
///     Page element definitions
///     Do not add logic here
/// </summary>
public class BookingListPage : VhAdminWebPage
{
    public static By SearchCaseTextBox = By.Id("caseNumber");

    public BookingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By VideoHearingsTable => By.Id("vh-table");

    public static By HearingDateTitle =>
        By.XPath($"//div[text()[contains(.,'{DateTime.Today.ToString("dd MMMM yyyy")}')]]");

    public static By HearingDetailsRow =>
        By.XPath("//div[@class='vh-row-created']//div[@class='govuk-grid-row vh-row vh-a']");

    public static By SearchButton => By.Id("searchButton");
    public static By ConfirmedButton => By.XPath("//*[contains(text(),'Confirmed')]");
    public static By TelephoneParticipantLink => By.XPath("//div[@id='conference_phone_details']");
    public static By VideoParticipantLink => By.XPath("//div[contains(text(),'video-participant-link')]");
    public static By SearchPanelButton => By.Id("openSearchPanelButton");
    public static By VenueListbox => By.Id("venues");
    public static By AllHearings => By.XPath("//div[@class='govuk-grid-row vh-row vh-a']");
    public static By StartDate => By.Id("startDate");
    public static By EndDate => By.Id("endDate");
    public static By CaseTypes => By.Id("caseTypes");

    public static By HearingDetailsRowSpecific(string caseNumber) =>
        By.XPath($"//div[text()[contains(.,'{caseNumber}')]]");

    public static By HearingSelectionSpecificRow(string caseNumber) => By.XPath(
        $"//div[@class='govuk-grid-row vh-row vh-a' and contains(.,'{caseNumber}')]//div[@class='vh-created-booking']");

    public static By VenueCheckbox(string venue) => By.XPath($"//input[@aria-label='Venue name {venue}']");
    public static By CaseTypeCheckbox(string caseType) => By.XPath($"//input[@aria-label='Case type {caseType}']");
}