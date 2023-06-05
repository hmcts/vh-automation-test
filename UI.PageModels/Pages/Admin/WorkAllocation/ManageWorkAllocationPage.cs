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
        private readonly ManageTeamSection _manageTeamSection;
        
        public ManageWorkAllocationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            _uploadHoursSection = new UploadHoursSection(driver, defaultWaitTime);
            _allocateJusticeUserToHearingSection = new AllocateJusticeUserToHearingSection(driver, defaultWaitTime);
            _manageTeamSection = new ManageTeamSection(driver, defaultWaitTime);
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

        public void AddTeamMember(string username, string firstName, string lastName, string contactTelephone,
            List<JusticeUserRoles> roles)
        {
            _manageTeamSection.EnterUserToSearchFor(username);
            _manageTeamSection.AddUserToTeam(username, firstName, lastName, contactTelephone, roles);
        }
        
        public void RestoreTeamMember(string username)
        {
            _manageTeamSection.EnterUserToSearchFor(username);
            _manageTeamSection.RestoreTeamMember(username);
        }

        public void EditTeamMember(string username, List<JusticeUserRoles> roles)
        {
            WaitForApiSpinnerToDisappear();
            _manageTeamSection.EnterUserToSearchFor(username);
            _manageTeamSection.EditExistingJusticeUserRole(username, roles);
        }
        
        public void DeleteTeamMember(string username)
        {
            _manageTeamSection.EnterUserToSearchFor(username);
            _manageTeamSection.DeleteExistingJusticeUser(username);
        }

        public void AllocateJusticeUserToHearing(string caseNumber = "automation test",
            string justiceUserDisplayName = "automation allocation")
        {
            _allocateJusticeUserToHearingSection.AllocateJusticeUserToHearing(caseNumber, justiceUserDisplayName);
        }
    }
}