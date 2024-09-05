namespace UI.PageModels.Pages.Admin.WorkAllocation;

public abstract class JusticeUserManagementPage : VhAdminWebPage
{
    private readonly ManageTeamSection _manageTeamSection;
    
    protected JusticeUserManagementPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        _manageTeamSection = new ManageTeamSection(driver, defaultWaitTime);
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
}

public class ManageTeamPage : JusticeUserManagementPage
{
    
    public ManageTeamPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForApiSpinnerToDisappear();
    }
} 

public class ManageWorkAllocationPage : JusticeUserManagementPage
{
    private readonly AllocateJusticeUserToHearingSection _allocateJusticeUserToHearingSection;
    private readonly EditHoursSection _editHoursSection;
    private readonly UploadHoursSection _uploadHoursSection;

    public ManageWorkAllocationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        _uploadHoursSection = new UploadHoursSection(driver, defaultWaitTime);
        _editHoursSection = new EditHoursSection(driver, defaultWaitTime);
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

    public void EditWorkHourForUser(string username, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        _editHoursSection.EditWorkingHours(username, dayOfWeek, startTime, endTime);
    }

    public void AddNonAvailableDayForUser(string username, DateTime startDateTime, DateTime endDateTime)
    {
        _editHoursSection.FilterForNonAvailableDays(username, startDateTime);
        _editHoursSection.AddNonAvailableDayForUser(username, startDateTime, endDateTime, searchForUser:false);
    }

    public void AllocateJusticeUserToHearing(string caseNumber = "Automation Test Hearing",
        string justiceUserDisplayName = "automation allocation", string justiceUserUsername = "automation.allocation@hearings.reform.hmcts.net")
    {
        _allocateJusticeUserToHearingSection.AllocateJusticeUserToHearing(caseNumber, justiceUserDisplayName, justiceUserUsername);
    }
}