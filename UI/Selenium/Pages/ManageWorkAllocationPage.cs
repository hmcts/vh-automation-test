using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using TestFramework;
using UI.Model;
using UI.Steps;

namespace UI.Pages
{
    ///<summary>
    ///   WorkAllocationPage
    ///   Page element definitions
    ///   Do not add logic here
    ///</summary>
    public class ManageWorkAllocationPage
    {
        public static By EditAvailability = By.Id("edit-availability");
        public static By ManageWorkAllocation = By.Id("manageWorkAllocationBtn");

        // Upload working hours availability
        public static By UploadWorkingHoursOrNonAvailability = By.Id("upload-availability");
        public static By UploadCSVFile = By.XPath("//input[@id='working-hours-file-upload']");
        public static By UploadAvailabilityHoursButton =
            By.CssSelector("#working-hours-file-upload-error .govuk-button");
        public static By TeamWorkingHoursUploadedSuccessfully = By.CssSelector("div#file-upload-result > p");
        public static By SuccessFileUpload = By.CssSelector("#file-upload-result:nth-of-type(2) p");
        public static By TeamWorkingHoursUploadedSuccessfullyM = By.CssSelector("#file-upload-result > p");
        public static By TeamNonAvailabilityHoursUploadedSuccessfully =
            By.CssSelector("div[id='file-upload-result'] p");

        // Edit  working hours /non availability 
        public static By EditWorkinghoursTab = By.CssSelector("app-edit-work-hours .govuk-details__summary");
        public static By EditWorkinghoursRadioButton = By.XPath("//label[@for='edit-working-hours']");
        public static By EditNonAvailabilityhoursRadioButton = By.Id("edit-non-availability-hours");
        public static By EditWorkingHoursNonAvailability = By.Id("edit-availability");
        public static By UploadNonAvailabilityHours = By.CssSelector("#non-availability-hours-file-upload");
        public static By UploadNonAvailabilityHoursButton =
            By.XPath("//div[@id='non-working-hours-file-upload-error']//button[.='Upload']");

        //Edit working hours non availability
        public static By EditSearchTeamMemberField = By.Id("username");
        public static By SearchFieldUsername = By.XPath("//input[@id='username']");
        public static By SearchButton = By.CssSelector(".govuk-grid-row:nth-child(1) > .govuk-button");
        public static By SetStartDate = By.Id("start-date");
        public static By SetEndDate = By.Id("end-date");
        public static By FilterDate = By.Id("filter-btn");

        // Manage team
      public static By ManageTeam = By.Id("manage-team");
    
     // public static By ManageTeamSearchTeamMemberField1 = By.Id("search-team-member");
        public static By ManageTeamSearchTeamMemberField = By.Id("search-team-member");
        public static By ManageTeamSearchButton =
            By.XPath("(//input[@id='search-team-member']//following::div/button)[1]");
        public static By SearchForTeamMember = By.CssSelector(".govuk-grid-column-one-third > .govuk-button");
        public static By ManageTeamNouserErrorMsg =
            By.XPath("//div[@class='govuk-!-font-weight-bold vh-text-color-red']");
        public static By ManageTeamAddNewTeamMember = By.XPath("//button[normalize-space()='Add a team member']");
        public static By ManageTeamAddJusticeUserPopUp = By.XPath("//form[@class='ng-untouched ng-pristine ng-valid']");

        // Add Justice User popup window elements

        public static By AddJusticeUserID= By.XPath("//fieldset[@class='govuk-fieldset']//input[@id='username']");
        public static By AddJusticeUserFirstName = By.XPath("//input[@id='firstName']");
        public static By AddJusticeUserLastName = By.XPath("//input[@id='lastName']");
        public static By AddJusticeUserContactNumber = By.XPath("//input[@id='contactTelephone']");
        public static By AddJusticeUserRole = By.XPath("//select[@id='role']");
        public static By AddJusticeUserSaveButton = By.XPath("//button[normalize-space()='Save']");
        public static By AddJusticeUserDiscardButton = By.XPath("//button[normalize-space()='Discard Changes']");
       // public static By VerifyJusticeUsername = By.XPath("//td[normalize-space()='" + WorkAllocationManageTeam._justiceUserName + "']");
       public static By VerifyExistJusticeUsername = By.XPath("//td[normalize-space()='" + WorkAllocationManageTeamEditUser._ValidJusticeUserName + "']");

       public static By ManageTeamEditRoleButton =
           By.XPath(
               "//td[@class='govuk-table__cell']//fa-icon[@class='ng-fa-icon']//*[name()='svg']");
        public static By ManageTeamDeleteUser = By.CssSelector("td:nth-child(7)");
        public static By ManageTeamDeletUserPopUpWindow =
            By.XPath(
                "//body[1]/app-root[1]/div[1]/div[2]/main[1]/app-work-allocation[1]/div[1]/app-manage-team[1]/details[1]/app-confirm-delete-justice-user-popup[1]/div[1]/div[1]/p[1]");
       // public static By VerifyDeleteUser =
         //   By.XPath("//a[normalize-space()='" + WorkAllocationManageTeam._justiceUserName + "']");
        public static By ManageTeamDeleteUserYesButton = By.XPath("//button[@id='btnConfirm']");
        public static By VerifyManageTeamDeleteUser = By.XPath("//span[@class='badge']");
        public static By ManageTeamRestoreUserButton =
            By.XPath("//td[@class='govuk-table__cell']//fa-icon[@class='ng-fa-icon']");
        public static By ManageTeamRestoreUserPopUpWindow = By.XPath("//div[@class='popup popup-small']");
        public static By VerifyManageTeamRestoreUserDetails =
            By.XPath(
                "//body[1]/app-root[1]/div[1]/div[2]/main[1]/app-work-allocation[1]/div[1]/app-manage-team[1]/details[1]/app-confirm-restore-justice-user-popup[1]/div[1]/div[1]/p[1]");
        public static By ManageTeamRestoreUserYesButton = By.XPath("//button[normalize-space()='Yes, proceed']");
        public static By VerifyManageTeamRestoreUserConfirmation = By.XPath("//div[@class='govuk-!-font-weight-bold']");



        // Allocate hearings
        public static By AllocateHearingsTab = By.XPath("//span[normalize-space()='Allocate hearings']");
        public static By AllocateHearings = By.Id("allocate-hearings");
        public static By AllocateHearingsEndDate = By.Id("to-date-entry");
        public static By AllocateHearingsFromDate = By.XPath("//input[@name='fromDate']")
            ;
        public static By AllocateHearingCSOSelectList = By.CssSelector("body > app-root:nth-child(1) > div:nth-child(1) > div:nth-child(3) > main:nth-child(1) > app-work-allocation:nth-child(2) > div:nth-child(2) > app-allocate-hearings:nth-child(4) > details:nth-child(1) > div:nth-child(3) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > app-justice-users-menu:nth-child(1) > div:nth-child(1) > ng-select:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > input:nth-child(1)");
        public static By AlloctatedCSOList =
            By.XPath(
                "//ng-select[@id='user-list']//div[@role='combobox']//following::ng-dropdown-panel//div[@role='option']");

        public static By AllocateHearingSearchButton =
            By.XPath("//button[@class='govuk-button govuk-!-margin-right-6']");

        public static By AllocateHearingCsoSelect = By.XPath("//input[@id='item-87']");
        
        
        
        public static By CaseType = By.Id("caseTypes");
        public static By CaseNumber = By.Id("case-number-entry");
        public static By AllocateHearingSearch =
            By.CssSelector("(//button[@data-module='govuk-button'][.='Search'])[3]");
        public static By AllocateTo = By.CssSelector("#user-menu #users");
        public static By OnlyAllocatedHearing = By.Id("is-unallocated");
        public static By Search =
            By.XPath(
                "//main[@id='main-content']/app-work-allocation/div/app-allocate-hearings/details/div/div[4]/button");

        public static By AllocateHearingSelectFirstCase = By.XPath("//input[@name='select-hearing_0']");
        public static By AllocateHearingSelectSecondCase = By.XPath("//input[@name='select-hearing_1']");
        public static By AllocateHearingConfirmButton = By.XPath("//button[normalize-space()='Confirm']");

        public static By VerifyAllocateHearingConfirmMsg = By.CssSelector(
            "div[class='govuk-body govuk-!-font-weight-bold govuk-grid-column-one-half govuk-!-padding-top-6']");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webdriver"></param>
        /// <param name="filePath"></param>
        public static void UploadWorkHoursFile(IWebDriver webdriver, string filePath)
        {
            var fullPath = System.IO.Path.GetFullPath(filePath);
            System.IO.File.Exists(filePath).Should().BeTrue();
            webdriver.FindElement(UploadCSVFile).SendKeys(fullPath);
            webdriver.FindElement(UploadAvailabilityHoursButton).Click();
            // Assert Message
            ExtensionMethods.WaitForElementVisible(webdriver, TeamWorkingHoursUploadedSuccessfully);
            webdriver.FindElement(TeamWorkingHoursUploadedSuccessfully).Should().NotBeNull();
        }

        public static void UploadNonWorkHoursFile(IWebDriver webdriver, string filePath)
        {
            var fullPath = System.IO.Path.GetFullPath(filePath);
            System.IO.File.Exists(filePath).Should().BeTrue();
            webdriver.FindElement(UploadNonAvailabilityHours).SendKeys(fullPath);
            webdriver.FindElement(UploadNonAvailabilityHoursButton).Click();
        }
    }
}