namespace UI.PageModels.Pages.Video.Participant;

public class StaffMemberHearingListPage : VhVideoWebPage
{
    public StaffMemberHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By HealingListRow => By.XPath("//tr[@class='govuk-table__row']");
}