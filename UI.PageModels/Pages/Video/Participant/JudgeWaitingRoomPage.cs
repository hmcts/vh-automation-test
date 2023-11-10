using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class JudgeWaitingRoomPage : VhVideoWebPage
{
    private readonly By _confirmStartHearingButton = By.Id("btnConfirmStart");

    private readonly By _startOrResumeHearingBtn =
        By.XPath("//button[normalize-space()='Start video hearing' or normalize-space()='Resume video hearing']");
    
    private readonly By _enterConsultationRoomBtn = By.Id("joinPCButton");
    private readonly By _editJudgeDisplayNameLink = By.Id("edit-judge-link");
    private readonly By _editJudgeDisplayNameTextBox = By.Id("new-judge-name");
    private readonly By _editJudgeDisplayNameSaveButton = By.Id("editJudgeDisplayName");
    private readonly By _editStaffMemberDisplayNameLink = By.Id("edit-staff-member-link");
    private readonly By _editStaffMemberDisplayNameTextBox = By.Id("new-staff-member-name");
    private readonly By _editStaffMemberDisplayNameSaveButton = By.Id("editStaffmemberDisplayName");

    public JudgeWaitingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        // the start/resume hearing button is not available when a hearing is closed
        WaitForElementToBeClickable(_enterConsultationRoomBtn, 60);
    }

    public JudgeHearingRoomPage StartOrResumeHearing()
    {
        ClickElement(_startOrResumeHearingBtn);
        ClickElement(_confirmStartHearingButton);
        return new JudgeHearingRoomPage(Driver, DefaultWaitTime);
    }

    public bool IsHearingClosed()
    {
        return IsElementVisible(By.XPath("//h1[normalize-space()='Hearing is closed']"));
    }
    
    public bool IsHearingPaused()
    {
        return IsElementVisible(By.XPath("//h1[normalize-space()='Hearing paused']"));
    }

    public string GetParticipantStatus(string fullName)
    {
        return GetText(By.XPath($"//dt[normalize-space()='{fullName}']/following-sibling::dd[2]//label"));
    }
    
    public void WaitForParticipantToBeConnected(string fullName)
    {
        var path = $"//dt[normalize-space()='{fullName}']/following-sibling::dd//label[normalize-space()='Connected']";
        WaitForElementToBeVisible(By.XPath(path));
    }

    public string GetVideoAccessPointStatus(string accessPointName)
    {
        return GetText(By.XPath($"//dt[normalize-space()='{accessPointName}']/following-sibling::dd//label"));
    }

    public int GetParticipantConnectedCount()
    {
        var connectedLabel = Driver.FindElements(By.XPath("//label[normalize-space()='Connected']"));
        return connectedLabel.Count;
    }

    public void ClearParticipantAddedNotification(string displayName)
    {
        var path =
            $"//div[descendant::span[text() = '{displayName}'] and descendant::span[text() = 'Participant has been added to the hearing.']]/following-sibling::div//button[.='OK']";
        ClickElement(By.XPath(path));
    }

    public void EditStaffMemberDisplayName()
    {
        ClickElement(_editStaffMemberDisplayNameLink);
        const string newName = "Edited Staff Member Name";
        EnterText(_editStaffMemberDisplayNameTextBox, newName);
        ClickElement(_editStaffMemberDisplayNameSaveButton);
    }

    public void EditJudgeDisplayName()
    {
        ClickElement(_editJudgeDisplayNameLink);
        const string newName = "Edited Judge Name";
        EnterText(_editJudgeDisplayNameTextBox, newName);
        ClickElement(_editJudgeDisplayNameSaveButton);
    }
}