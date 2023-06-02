using FluentAssertions;
using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.WorkAllocation
{
    ///<summary>
    ///   WorkAllocationPage
    ///   Page element definitions
    ///   Do not add logic here
    ///</summary>
    public class ManageWorkAllocationPage : VhAdminWebPage
    {
        private readonly By _editAvailability = By.Id("edit-availability");
        private readonly By _manageWorkAllocation = By.Id("manageWorkAllocationBtn");

        // Upload working hours availability
        private readonly By _uploadHoursSectionBtn = By.Id("upload-availability");
        
        private readonly By _uploadWorkHoursCsvFileField = By.XPath("//input[@id='working-hours-file-upload']");
        private readonly By _uploadAvailabilityHoursButton =By.CssSelector("#working-hours-file-upload-error .govuk-button");
        
        private readonly By _uploadNonAvailabilityCsvField = By.CssSelector("#non-availability-hours-file-upload");
        private readonly By _uploadNonAvailabilityHoursButton =
            By.XPath("//div[@id='non-working-hours-file-upload-error']//button[.='Upload']");

        private readonly By _teamWorkingHoursUploadedSuccessfully = By.CssSelector("div#file-upload-result > p");

        // Allocate hearings section
        private readonly By _allocateHearingsSectionBtn = By.Id("allocate-hearings");

        public ManageWorkAllocationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            WaitForApiSpinnerToDisappear();
            WaitForElementToBeClickable(_uploadHoursSectionBtn);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webdriver"></param>
        /// <param name="filePath"></param>
        public void UploadWorkHoursFile(string filePath)
        {
            ClickElement(_uploadHoursSectionBtn);
            var fullPath = Path.GetFullPath(filePath);
            File.Exists(filePath).Should().BeTrue();
            EnterText(_uploadWorkHoursCsvFileField, fullPath);
            ClickElement(_uploadAvailabilityHoursButton);
        }
        
        public void WaitForFileUploadSuccessMessage()
        {
            WaitForElementToBeVisible(_teamWorkingHoursUploadedSuccessfully);
        }

        public void UploadNonWorkHoursFile(string filePath)
        {
            ClickElement(_uploadHoursSectionBtn);
            var fullPath = Path.GetFullPath(filePath);
            File.Exists(filePath).Should().BeTrue();
            EnterText(_uploadNonAvailabilityCsvField, fullPath);
            ClickElement(_uploadNonAvailabilityHoursButton);
        }
        
        public void AllocateJusticeUserToHearing(string caseNumber = "automation test", string justiceUserDisplaName = "automation allocation")
        {
            WaitForApiSpinnerToDisappear();
            // open the section
            ClickElement(_allocateHearingsSectionBtn);
            
            // enter the case number
            var caseNumberField = By.Id("case-number-entry");
            EnterText(caseNumberField, caseNumber);
            
            // search for hearings
            var searchButton = By.Id("allocate-hearings-search-btn");
            ClickElement(searchButton);
            
            // search for justice user
            // var selectorDropdown = By.Id("select-menu");
            // ClickElement(selectorDropdown);
            //*[@id='select-menu']/div/div/div[2]/[@type='text']
            var justiceUserSelectDropdown = By.XPath("//app-select[@id='select-cso-search-allocation']//input[@type='text']");
            ClickElement(justiceUserSelectDropdown);
            EnterText(justiceUserSelectDropdown, justiceUserDisplaName, false);

            var firstName = justiceUserDisplaName.Split(" ")[0];
            var dropDownSelector = By.XPath($"//input[@aria-label='User {firstName}']");
            ClickElement(dropDownSelector);
            var selectorArrowBtn =
                By.XPath("//app-select[@id='select-cso-search-allocation']//span[@class='ng-arrow-wrapper']");
            ClickElement(selectorArrowBtn);
            
            // click first checkbox on the results table list
            var firstCheckbox = By.CssSelector("input[name='select-hearing_0']");
            ClickElement(firstCheckbox);
            
            
            var allocateButton = By.Id("confirm-allocation-btn");
            ClickElement(allocateButton);
            WaitForApiSpinnerToDisappear();

            var actionsMessageContainer = By.Id("allocation-actions-message-container");
            GetText(actionsMessageContainer).Should().Contain("Hearings have been updated");
        }
    }
}