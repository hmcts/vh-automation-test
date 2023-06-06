using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Vho;

public class VhoVenueSelectionPage : VhPage
{
    public VhoVenueSelectionPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By HearingList => By.CssSelector("input[aria-autocomplete='list']");
    public static By HearingCheckBox => By.CssSelector("input[type='checkbox']");
    public static By ViewHearings => By.CssSelector("#select-venue-allocation-btn");
    public static By HearingBtn => By.Id("hearingsTabButton");
    public static By FailedAlert => By.CssSelector("div#tasks-list div.govuk-grid-row");
    public static By SelectCaseNumber(string caseNumber) => By.XPath($"//div[contains(text(),'{caseNumber}')]");

    public static By AlertMsg(string rowNum) =>
        By.CssSelector($"div#tasks-list div.govuk-grid-row:nth-child({rowNum}) .task-body");

    public static By FirstLastName(string rowNum) =>
        By.CssSelector($"div#tasks-list div.govuk-grid-row:nth-child({rowNum}) .task-origin");
}