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
    
        // Upload working hours availability
        public static By UploadWorkingHoursOrNonAvailability = By.Id("upload-availability");
        public static By UploadCSVFile = By.XPath("//input[@id='working-hours-file-upload']");
        public static By UploadAvailabilityHoursButton = By.CssSelector("#working-hours-file-upload-error .govuk-button");
        public static By Nonresult = By.CssSelector("div#file-upload-result > p");
        public static By SuccessFileUpload = By.CssSelector("#file-upload-result:nth-of-type(2) p");
        public static By FileUploadResult = By.CssSelector("#file-upload-result > p");
        public static By result = By.CssSelector("#file-upload-result > p");
        
        // Edit  = rking hours /non availability 
        public static By EditWorkinghoursTab = By.CssSelector("app-edit-work-hours .govuk-details__summary");
        public static By EditWorkinghoursRadioButton = By.XPath("//label[@for='edit-working-hours']");
        public static By EditNonAvailabilityhoursRadioButton = By.Id("edit-non-availability-hours");
        public static By EditWorkingHoursNonAvailability = By.Id("edit-availability");
        public static By UploadNonAvailabilityHours = By.CssSelector("#non-availability-hours-file-upload");
        public static By UploadNonAvailabilityHoursButton = By.XPath("//div[@id='non-working-hours-file-upload-error']//button[.='Upload']");

        //Edit working hours non availability
        public static By EditSearchTeamMemberField = By.Id("username");
        public static By SearchFieldUsername = By.XPath("//input[@id='username']");
        public static By SearchButton = By.CssSelector(".govuk-grid-row:nth-child(1) > .govuk-button");
        public static By SetStartDate = By.Id("start-date");
        public static By SetEndDate = By.Id("end-date");
        public static By FilterDate = By.Id("filter-btn");
    
        // Manage team
        public static By ManageTeam = By.Id("manage-team");
        public static By ManageTeamSearchTeamMemberField1 = By.Id("search-team-member");
        public static By ManageTeamSearchTeamMemberField = By.Id("username");
        public static By ManageTeamSearchButton = By.XPath("(//input[@id='search-team-member']//following::div/button)[1]");
        public static By SearchForTeamMember = By.CssSelector(".govuk-grid-column-one-third > .govuk-button");
        // Allocate hearings
        public static By AllocateHearingsTab = By.CssSelector("css=app-allocate-hearings .govuk-details__summary");
        public static By AllocateHearings = By.Id("allocate-hearings");
        public static By HearingRangeStartDate = By.Id("id=from-date-entry");
        public static By HearingRangeEndDate = By.Id("id=to-date-entry");
        public static By HearingDate = By.Id("hearing-date-entry");
        public static By CTSCOfficerUserName = By.Id("Ctsc-officer-user-name-entry");
        public static By CaseNumber = By.Id("case-number-entry");
        public static By AllocateHearingSearch = By.XPath("//main[@id='main-content']/app-work-allocation/div/details[3]/div/div[4]/button");
        public static By AllocateTo = By.CssSelector("#user-menu #users");
        public static By Search = By.XPath("//main[@id='main-content']/app-work-allocation/div/app-allocate-hearings/details/div/div[4]/button");

    }
}
