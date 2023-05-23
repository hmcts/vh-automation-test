using FluentAssertions;
using OpenQA.Selenium;
using TestFramework;
using UI.PageModels.Pages.Video;

namespace UI.Pages
{
    ///<summary>
    ///   WorkAllocationPage
    ///   Page element definitions
    ///   Do not add logic here
    ///</summary>
    public class ManageWorkAllocationPage : VhPage
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
        // private readonly By _successFileUpload = By.CssSelector("#file-upload-result:nth-of-type(2) p");
        // private readonly By _teamWorkingHoursUploadedSuccessfullyM = By.CssSelector("#file-upload-result > p");
        // private readonly By _teamNonAvailabilityHoursUploadedSuccessfully =
        //     By.CssSelector("div[id='file-upload-result'] p");

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
    }
}