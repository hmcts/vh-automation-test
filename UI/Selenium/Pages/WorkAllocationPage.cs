using OpenQA.Selenium;

namespace UI.Pages
{
    ///<summary>
    ///   WorkAllocationPage
    ///   Page element definitions
    ///   Do not add logic here
    ///</summary>
    public class WorkAllocationPage
    {
        public static By EditAvailability = By.Id("edit-availability"); 
        public static By ManageWorkAllocationMainButton => By.XPath("//div[@id='manageWorkAllocationBtn']"); 
        
        // Manage Work Allocation Page Links
        
        public static By UploadWorkingNonAvailabilityHours => By.XPath("//span[@id='upload-availability']");
        public static By EditWorkingNonAvailabilityHours => By.XPath("//span[@id='edit-availability']"); 
        public static By ManageTeam => By.XPath("//span[@id='manage-team']"); 
        public static By AllocateHearing => By.XPath("//span[@id='allocate-hearings']"); 
        
        //Upload Working Hours Child Links 
        
        public static By UploadWorkingHoursChooseFileButton => By.XPath("//input[@id='working-hours-file-upload']");
        public static By UploadWorkingHoursFileUploadButton => By.CssSelector("div[id='working-hours-file-upload-error'] button[class='govuk-button govuk-!-margin-left-6']");
        public static By UploadNonAvailabilityHoursChooseFileButton => By.XPath("//input[@id='non-availability-hours-file-upload']]");
        public static By UploadNonAvailabilityHoursUploadButton => By.CssSelector("div[id='non-working-hours-file-upload-error'] button[class='govuk-button govuk-!-margin-left-6']");
        
        //Edit Working Hours Child Links 
        
        public static By EditWorkingHoursSelect => By.XPath("//input[@id='edit-working-hours']");
        public static By EditNonAvailabilityHourSelect => By.XPath("//input[@id='edit-non-availability-hours']");
        public static By EditHourSearchTeamMemberBox => By.XPath("//input[@id='username']");
        public static By EditHourSearchButton => By.XPath("//div[@class='govuk-form-group govuk-!-margin-left-6']//button[@class='govuk-button'][normalize-space()='Search']");
        
        //Manage Team Child Links 
        
        public static By ManageTeamSearchTeamBox => By.XPath("//input[@id='search-team-member']");
        public static By ManageTeamSearchButton => By.XPath("//div[@class='govuk-grid-column-one-third']//button[@class='govuk-button'][normalize-space()='Search']]");
                // Search Results Buttons Links >> To select particular User we obtain all elements of Edit and Delete Button elements in lists and then iterate the loop to select particular one 
                //   Xpath for Table => //table[@data-module='moj-sortable-table'] CSS for Table =>.govuk-table[data-module='moj-sortable-table']
                
        // Allocate Hearing Child Links
        
        public static By AllocateHearingDateRangeStartDate => By.XPath("//input[@id='from-date-entry']");
        public static By AllocateHearingDateRangeEndDate => By.XPath("//input[@id='to-date-entry']");
        public static By AllocateCSOOptionBox => By.XPath("//input[@id='users']");
        public static By AllocateHearingCaseTypeSelectOptions => By.XPath("//input[@id='caseTypes']");
        public static By AllocateHearingUnallocatedHearingSelectOption=> By.XPath("//input[@id='is-unallocated']");
        public static By AllocateHearingSearchButton=> By.XPath("//button[@class='govuk-button govuk-!-margin-right-6'][normalize-space()='Search']");
        public static By AllocateHearingClearButton=> By.XPath("//button[normalize-space()='Clear']");
        
        //  Xpath => //table[@aria-describedby='table of hearings with their assigned CSOs']
        
        
        


















    }
}
