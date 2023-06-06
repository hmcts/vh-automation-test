using FluentAssertions;
using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.WorkAllocation;

public enum JusticeUserRoles
{
    Vho,
    VhTeamLead,
    StaffMember
}

public class ManageTeamSection : VhAdminWebPage
{
    private readonly By _manageTeamSectionBtn = By.Id("manage-team");

    private readonly By _searchForUserField = By.Id("search-team-member");
    private readonly By _searchForUserBtn = By.Id("manage-team-search-for-user-btn");
    private readonly By _addUserBtn = By.Id("manage-team-add-user-btn");
    private readonly By _messagesContainer = By.Id("manage-team-messages-container");
    
    private readonly By _searchResultsTable = By.Id("manage-team-justice-user-search-results-table");

    private readonly By _editRowLocator =
        By.XPath("//td[@class='govuk-table__cell']//fa-icon[@class='ng-fa-icon']//*[name()='svg']");
    private readonly By _deleteRowLocator = By.CssSelector("td:nth-child(7)");
    private readonly By _restoreRowLocator = By.CssSelector("td:nth-child(7)"); // delete disappears and restore appears when a user has been deleted
    
    private readonly By _confirmDeleteBtn = By.Id("btnConfirm");
    private readonly By _cancelDeleteBtn = By.Id("btnCancel");
    
    // justice user form
    private readonly By _justiceUserForm = By.TagName("app-justice-user-form");
    private readonly By _justiceUserFormUsername = By.XPath("//form[@id='justice-user-form']//input[@id='username']");
    private readonly By _justiceUserFormFirstName = By.XPath("//input[@id='firstName']");
    private readonly By _justiceUserFormLastName = By.XPath("//input[@id='lastName']");
    private readonly By _justiceUserFormContactTelephone = By.XPath("//input[@id='contactTelephone']");

    private readonly By _confirmDeleteJusticeUserPopup = By.TagName("app-confirm-delete-justice-user-popup");
    private readonly By _saveJusticeUserFormBtn = By.Id("justice-user-form-save-btn");
    private readonly By _discardJusticeUserFormBtn = By.Id("justice-user-form-discard-btn");
    
    private readonly By _confirmRestoreJusticeUserPopup = By.TagName("app-confirm-restore-justice-user-popup");
    private readonly By _confirmRestoreBtn = By.Id("btnConfirm");
    private readonly By _cancelRestoreBtn = By.Id("btnCancel");

    

    public ManageTeamSection(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public void EnterUserToSearchFor(string userDisplayName = "automation test")
    {
        CheckSectionIsOpen();
        WaitForApiSpinnerToDisappear();
        
        EnterText(_searchForUserField, userDisplayName);
        ClickElement(_searchForUserBtn);

    }

    public void AddUserToTeam(string username, string firstName, string lastName, string contactTelephone, List<JusticeUserRoles> roles)
    {
        CheckSectionIsOpen();
        WaitForApiSpinnerToDisappear();

        EnterUserToSearchFor(username);
        WaitForElementToBeVisible(_messagesContainer);
        GetText(_messagesContainer).Should().Contain("No users matching this search criteria were found");

        ClickElement(_addUserBtn);

        WaitForElementToBeVisible(_justiceUserForm);
        EnterNewJusticeUserForm(username, firstName, lastName, contactTelephone, roles);
    }

    private void EnterNewJusticeUserForm(string username, string firstName, string lastName, string contactTelephone,
        List<JusticeUserRoles> roles)
    {
        EnterText(_justiceUserFormUsername, username);
        EnterText(_justiceUserFormFirstName, firstName);
        EnterText(_justiceUserFormLastName, lastName);
        if (!string.IsNullOrWhiteSpace(contactTelephone))
        {
            EnterText(_justiceUserFormContactTelephone, contactTelephone);
        }
        
        SelectRoles(roles);
        
        SaveForm();
        if (HasFormValidationError())
        {
            throw new Exception("Add Justice User Form has validation error");
        }
    }

    private void SelectRoles(List<JusticeUserRoles> roles)
    {
        
        foreach (var justiceUserRole in Enum.GetValues<JusticeUserRoles>())
        {
            var locator = By.Id($"role_{justiceUserRole.ToString()}");
            var hasRole = roles.Contains(justiceUserRole);
            SetCheckboxValue(locator, hasRole);
        }
    }
    
    public void EditExistingJusticeUserRole(string username, List<JusticeUserRoles> roles)
    {
        var row = GetRowForByUsername(username);
        var editButton = row.FindElement(_editRowLocator);
        editButton.Click();
        WaitForElementToBeVisible(_justiceUserForm);
        SelectRoles(roles);
        SaveForm();
    }

    public void DeleteExistingJusticeUser(string username)
    {
        var row = GetRowForByUsername(username);
        var deleteCell = row.FindElement(_deleteRowLocator);
        var deleteButton = deleteCell.FindElement(By.XPath("//fa-icon[@class='ng-fa-icon red-trash']"));
        deleteButton.Click();
        WaitForElementToBeVisible(_confirmDeleteJusticeUserPopup);
        ClickElement(_confirmDeleteBtn);
        WaitForApiSpinnerToDisappear();
        
        // check row has the deleted badge
        row = GetRowForByUsername(username);
        var deletedBadge = row.FindElement(By.XPath("//span[@class='badge']"));
        deletedBadge.Should().NotBeNull();
        deletedBadge.Text.Should().Be("Deleted");
    }

    public void RestoreTeamMember(string username)
    {
        var row = GetRowForByUsername(username);
        var restoreButtonCell = row.FindElement(_restoreRowLocator);
        var restoreButton = restoreButtonCell.FindElement(By.XPath("//fa-icon[@class='ng-fa-icon']"));
        restoreButton.Click();
        WaitForElementToBeVisible(_confirmRestoreJusticeUserPopup);
        ClickElement(_confirmDeleteBtn);
        WaitForApiSpinnerToDisappear();
    }

    private IWebElement GetRowForByUsername(string username)
    {
        // the cell contains the username or is prefixed with Deleted
        var locator = By.XPath($"//tr[.//td[normalize-space()='{username}']] | //tr[.//td[normalize-space()='Deleted{username}']]");
        var elem =  Driver.FindElement(locator);
        return elem;
    }

    private void CheckSectionIsOpen()
    {
        if (IsElementVisible(_searchForUserBtn)) return;
        ClickElement(_manageTeamSectionBtn);
    }

    private void SaveForm()
    {
        ClickElement(_saveJusticeUserFormBtn);
        WaitForApiSpinnerToDisappear();
    }
}