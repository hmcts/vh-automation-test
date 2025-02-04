namespace UI.PageModels.Pages.Video.Participant;

public class JudgeWaitingRoomPage(IWebDriver driver, int defaultWaitTime) : CommonWaitingRoomPage(driver, defaultWaitTime)
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

    protected override void ConfirmPageHasLoaded()
    {
        // the start/resume hearing button is not available when a hearing is closed
        WaitForElementToBeClickable(_enterConsultationRoomBtn, DefaultWaitTime);
    }

    public JudgeHearingRoomPage StartOrResumeHearing()
    {
        ClickElement(_startOrResumeHearingBtn);
        WaitForElementToBeClickable(_confirmStartHearingButton);
        Driver.TakeScreenshotAndSave(GetType().Name, "Prompted to confirm start/resume hearing");
        ClickElement(_confirmStartHearingButton);
        Driver.TakeScreenshotAndSave(GetType().Name, "Confirmed start/resume hearing");
        return new JudgeHearingRoomPage(Driver, DefaultWaitTime);
    }

    public bool IsHearingClosed()
    {
        var hearingClosed = By.XPath("//h1[normalize-space()='Hearing is closed']");
        WaitForElementToBeVisible(hearingClosed);
        Driver.TakeScreenshotAndSave(GetType().Name, "Hearing closed message displayed");
        return IsElementVisible(hearingClosed);
    }
    
    public bool IsHearingPaused()
    {
        return IsElementVisible(By.XPath("//h1[normalize-space()='Hearing paused']"));
    }

    /// <summary>
    /// Get non staff member participant status
    /// </summary>
    /// <param name="participantId"></param>
    /// <returns></returns>
    public string GetParticipantStatus(Guid participantId)
    {
        return GetText(By.Id($"p-{participantId}-status-participant"));
    }
    
    public void WaitForParticipantToBeConnected(string fullName)
    {
        var path = $"//dl[contains(., '{fullName}')]//span[contains(text(), 'Connected')]";
        WaitForElementToBeVisible(By.XPath(path));
    }
    
    /// <summary>
    /// Use the participant id to wait for the participant to be connected (from Video API)
    /// </summary>
    /// <param name="participantId"></param>
    public void WaitForParticipantToBeConnectedById(string participantId)
    {
        var path = $"//span[@id='p-{participantId}-status-participant'][.=' Connected ']";
        WaitForElementToBeVisible(By.XPath(path));
    }

    public string GetVideoAccessPointStatus(string accessPointName)
    {
        return GetText(By.XPath($"//dt[normalize-space()='{accessPointName}']/following-sibling::dd//span"));
    }

    public int GetParticipantConnectedCount()
    {
        var connectedLabel = Driver.FindElements(By.XPath("//span[normalize-space()='Connected']"));
        return connectedLabel.Count;
    }

    public void EditStaffMemberDisplayName(string newName = "Edited Staff Member Name")
    {
        ClickElement(_editStaffMemberDisplayNameLink);
        EnterText(_editStaffMemberDisplayNameTextBox, newName);
        ClickElement(_editStaffMemberDisplayNameSaveButton);
    }

    public void EditJudgeDisplayName(string newName = "Edited Judge Name")
    {
        ClickElement(_editJudgeDisplayNameLink);
        EnterText(_editJudgeDisplayNameTextBox, newName);
        ClickElement(_editJudgeDisplayNameSaveButton);
    }

    public string GetStaffMemberDisplayNameInWaitingRoom()
    {
        var nameElement = Driver.FindElement(By.XPath("//dt[contains(@id,'name-staff-member')]"));
        var name = ((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].firstChild.textContent;", nameElement).ToString();
        return name?.Trim() ?? string.Empty;
    }

    public bool ParticipantExistsInWaitingRoom(string displayName)
    {
        var htmlElement = Driver.FindElement(By.CssSelector("app-judge-participant-status-list"));
        var htmlContent = htmlElement.GetAttribute("outerHTML");
        
        return htmlContent.Contains(displayName);
    }

    public bool ParticipantExistsInHearingRoom(string displayName)
    {
        var locator = By.XPath($"//span[@class='wrap-anywhere'][normalize-space()='{displayName}']");
        return IsElementVisible(locator);
    }

    public string GetConsultationCloseTime()
    {
        return GetText(By.XPath("//span[@id='hearing-consultation-closing-time']/parent::strong"));
    }

    public ConsultationRoomPage JoinJudicialConsultationRoom()
    {
        ClickElement(_enterConsultationRoomBtn);
        return new ConsultationRoomPage(Driver, DefaultWaitTime);
    }
}