using OpenQA.Selenium;

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
    
        // Upload working hours or non availability
        public static By UploadWorkingHoursOrNonAvailability = By.Id("upload-availability");
        public static By UploadCSVFile = By.XPath("//input[@id='working-hours-file-upload']");
        public static By UploadAvailabilityHoursButton = By.CssSelector("#working-hours-file-upload-error .govuk-button");
        public static By FileUploadResult = By.CssSelector("#file-upload-result > p");
        public static By result = By.CssSelector("#file-upload-result > p");
       
        public static By UploadNonAvailabilityHoursButton = By.CssSelector("#non-working-hours-file-upload-error .govuk-button");
        
        // Edit working hours non availability 
        public static By EditWorkingHoursNonAvailability = By.Id("edit-availability");
        public static By EditWorkinghoursRadioButton = By.Id("edit-working-hours");
        public static By UploadNonAvailabilityHours = By.Name("non-availability-hours-file-upload");
        public static By EditNonAvailabilityhoursRadioButton = By.Id("edit-non-availability-hours");
        public static By Nonresult = By.CssSelector("div#file-upload-result > p");
        
        //div#file-upload-result > p
        public static By EditSearchTeamMemberField = By.Id("username");
        public static By SearchFieldUsername = By.XPath("//input[@id='username']");
        public static By SearchButton = By.CssSelector(".govuk-grid-row:nth-child(1) > .govuk-button");
    
        // Manage team
        public static By ManageTeam = By.Id("manage-team");
        public static By ManageTeamSearchTeamMemberField = By.Id("search-team-member");
        public static By ManageTeamSearchButton = By.CssSelector(".govuk-grid-row:nth-child(2) > .govuk-button");
    
        // Allocate hearings
        public static By AllocateHearings = By.Id("allocate-hearings");
        public static By HearingDate = By.Id("hearing-date-entry");
        public static By CTSCOfficerUserName = By.Id("Ctsc-officer-user-name-entry");
        public static By CaseNumber = By.Id("case-number-entry");
        public static By AllocateHearingSearch = By.XPath("//main[@id='main-content']/app-work-allocation/div/details[3]/div/div[4]/button");
        public static By AllocateHearingClear = By.XPath("//button[contains(.,'Clear')]");

    }
}
