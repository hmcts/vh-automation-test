using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   StaffMemberHearingListPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class StaffMemberHearingListPage
    {
        public static By HealingListRow => By.XPath("//tr[@class='govuk-table__row']");
    }
}
