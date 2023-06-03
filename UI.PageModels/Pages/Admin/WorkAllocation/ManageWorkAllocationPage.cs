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
        private readonly UploadHoursSection _uploadHoursSection;
        private readonly AllocateJusticeUserToHearingSection _allocateJusticeUserToHearingSection;
        
        public ManageWorkAllocationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            _uploadHoursSection = new UploadHoursSection(driver, defaultWaitTime);
            _allocateJusticeUserToHearingSection = new AllocateJusticeUserToHearingSection(driver, defaultWaitTime);
            WaitForApiSpinnerToDisappear();
        }

        public void UploadWorkHoursFile(string filePath)
        {
            _uploadHoursSection.UploadWorkHoursFile(filePath);

        }

        public void UploadNonWorkHoursFile(string filePath)
        {
            _uploadHoursSection.UploadNonWorkHoursFile(filePath);
        }

        public void AllocateJusticeUserToHearing(string caseNumber = "automation test",
            string justiceUserDisplayName = "automation allocation")
        {
            _allocateJusticeUserToHearingSection.AllocateJusticeUserToHearing(caseNumber, justiceUserDisplayName);
        }
    }
}