using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin.WorkAllocation;

public class AllocateJusticeUserToHearingSection : VhAdminWebPage
{
    private readonly By _actionsMessageContainer = By.Id("allocation-actions-message-container");
    private readonly By _allocateHearingBtn = By.Id("confirm-allocation-btn");
    private readonly By _allocateHearingsSectionBtn = By.Id("allocate-hearings");

    // search filter fields
    private readonly By _caseNumberField = By.Id("case-number-entry");
    private readonly By _isUnallocatedOnlyCheckbox = By.CssSelector("label[for='is-unallocated']");

    private readonly By _justiceUserSelectDropdownArrow =
        By.XPath("//app-select[@id='select-cso-search-allocation']//span[@class='ng-arrow-wrapper']");


    // allocate form and table
    private readonly By _justiceUserSelectDropdownTextBox =
        By.XPath("//app-select[@id='select-cso-search-allocation']//input[@type='text']");

    private readonly By _searchButton = By.Id("allocate-hearings-search-btn");

    public AllocateJusticeUserToHearingSection(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public void EnterSearchFilter(string caseNumber = "automation test", bool unallocatedOnly = false)
    {
        CheckSectionIsOpen();
        WaitForApiSpinnerToDisappear();
        // open the section
        ClickElement(_allocateHearingsSectionBtn);

        // enter the case number
        EnterText(_caseNumberField, caseNumber);

        if (unallocatedOnly)
        {
            ClickElement(_isUnallocatedOnlyCheckbox);
        }

        // search for hearings
        ClickElement(_searchButton);
    }


    public void AllocateJusticeUserToHearing(string caseNumber = "automation test",
        string justiceUserDisplayName = "automation allocation",
        string justiceUserUsername = "automation.allocation@hearings.reform.hmcts.net")
    {
        CheckSectionIsOpen();
        WaitForApiSpinnerToDisappear();

        // enter the case number
        EnterSearchFilter(caseNumber, true);

        ClickElement(_justiceUserSelectDropdownTextBox);
        EnterText(_justiceUserSelectDropdownTextBox, justiceUserDisplayName, false);

        var firstName = justiceUserDisplayName.Split(" ")[0];
        var dropDownSelector = By.XPath($"//input[@aria-label='User {firstName}']");
        ClickElement(dropDownSelector);

        ClickElement(_justiceUserSelectDropdownArrow);
        var firstCheckbox = By.CssSelector("input[name='select-hearing_0']");
        ClickElement(firstCheckbox);
        

        ClickElement(_allocateHearingBtn);
        WaitForApiSpinnerToDisappear();

        if (!GetText(_actionsMessageContainer).Contains("Hearings have been updated"))
        {
            throw new Exception("Hearings have not been updated");
        }
    }
    
    private IWebElement GetHearingCheckboxNotAllocatedToUser(string justiceUserUsername)
    {
        var row = Driver.FindElement(
            By.XPath($"//tr[.//td[7][normalize-space()!='{justiceUserUsername}']]"));
        return row.FindElement(By.XPath("//tbody/tr[2]/td[1]/input[1]"));;
    }

    private void CheckSectionIsOpen()
    {
        WaitForApiSpinnerToDisappear();

        if (!IsElementVisible(_allocateHearingBtn)) ClickElement(_allocateHearingsSectionBtn);
    }
}