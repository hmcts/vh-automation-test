using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using TestFramework;
using UI.Model;

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
        

        // Allocate hearings
        public static By AllocateHearingsTab = By.CssSelector("css=app-allocate-hearings .govuk-details__summary");
        public static By AllocateHearings = By.Id("allocate-hearings");
        public static By HearingRangeStartDate = By.Id("from-date-entry");
        public static By HearingRangeEndDate = By.Id("to-date-entry");
        public static By HearingDate = By.Id("hearing-date-entry");
        public static By AlloctatedCSO = By.XPath("//ng-select[@id='user-list']//div[@role='combobox']");
        public static By AlloctatedCSOList =
            By.XPath(
                "//ng-select[@id='user-list']//div[@role='combobox']//following::ng-dropdown-panel//div[@role='option']");
        public static By CaseType = By.Id("caseTypes");
        public static By CaseNumber = By.Id("case-number-entry");
        public static By AllocateHearingSearch =
            By.CssSelector("(//button[@data-module='govuk-button'][.='Search'])[3]");
        public static By AllocateTo = By.CssSelector("#user-menu #users");
        public static By OnlyAllocatedHearing = By.Id("is-unallocated");
        public static By Search =
            By.XPath(
                "//main[@id='main-content']/app-work-allocation/div/app-allocate-hearings/details/div/div[4]/button");

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