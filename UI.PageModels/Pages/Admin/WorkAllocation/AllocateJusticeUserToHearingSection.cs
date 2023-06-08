using FluentAssertions;
using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin.WorkAllocation;

public class AllocateJusticeUserToHearingSection : VhAdminWebPage
{
    private readonly By _actionsMessageContainer = By.Id("allocation-actions-message-container");
    private readonly By _allocateHearingBtn = By.Id("confirm-allocation-btn");
    private readonly By _allocateHearingsSectionBtn = By.Id("allocate-hearings");

    // search filter fields
    private readonly By _caseNumberField = By.Id("case-number-entry");

    private readonly By _justiceUserSelectDropdownArrow =
        By.XPath("//app-select[@id='select-cso-search-allocation']//span[@class='ng-arrow-wrapper']");


    // allocate form and table
    private readonly By _justiceUserSelectDropdownTextBox =
        By.XPath("//app-select[@id='select-cso-search-allocation']//input[@type='text']");

    private readonly By _searchButton = By.Id("allocate-hearings-search-btn");

    public AllocateJusticeUserToHearingSection(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public void EnterSearchFilter(string caseNumber = "automation test")
    {
        CheckSectionIsOpen();
        WaitForApiSpinnerToDisappear();
        // open the section
        ClickElement(_allocateHearingsSectionBtn);

        // enter the case number
        EnterText(_caseNumberField, caseNumber);

        // search for hearings
        ClickElement(_searchButton);
    }


    public void AllocateJusticeUserToHearing(string caseNumber = "automation test",
        string justiceUserDisplayName = "automation allocation")
    {
        CheckSectionIsOpen();

        WaitForApiSpinnerToDisappear();

        // enter the case number
        EnterText(_caseNumberField, caseNumber);

        // search for hearings
        ClickElement(_searchButton);

        ClickElement(_justiceUserSelectDropdownTextBox);
        EnterText(_justiceUserSelectDropdownTextBox, justiceUserDisplayName, false);

        var firstName = justiceUserDisplayName.Split(" ")[0];
        var dropDownSelector = By.XPath($"//input[@aria-label='User {firstName}']");
        ClickElement(dropDownSelector);

        ClickElement(_justiceUserSelectDropdownArrow);

        // click first checkbox on the results table list
        var firstCheckbox = By.CssSelector("input[name='select-hearing_0']");
        ClickElement(firstCheckbox);

        ClickElement(_allocateHearingBtn);
        WaitForApiSpinnerToDisappear();

        GetText(_actionsMessageContainer).Should().Contain("Hearings have been updated");
    }

    private void CheckSectionIsOpen()
    {
        WaitForApiSpinnerToDisappear();

        if (!IsElementVisible(_allocateHearingBtn)) ClickElement(_allocateHearingsSectionBtn);
    }
}