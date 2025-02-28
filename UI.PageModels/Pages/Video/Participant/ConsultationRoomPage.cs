namespace UI.PageModels.Pages.Video.Participant;

public class ConsultationRoomPage : VhVideoWebPage

{
    public ConsultationRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(LeaveButtonDesktop);
    }

    private static By ConfirmLeaveButton => By.Id("consultation-leave-button");
    private static By LeaveButtonDesktop => By.Id("leaveButton-desktop");

    private static By InviteParticipantButton(string name) =>
        By.XPath($"//div[@class='participant-endpoint-row' and contains(.,'{name}')]//app-invite-participant");

    /// <summary>
    /// Leave the consultation room and return to the waiting room
    /// </summary>
    /// <returns>A waiting room instance</returns>
    public ParticipantWaitingRoomPage LeaveConsultationRoom()
    {
        LeaveConsultationRoomWithConfirmation();
        return new ParticipantWaitingRoomPage(Driver, DefaultWaitTime);
    }

    public JudgeWaitingRoomPage LeaveJudicialConsultationRoom()
    {
        LeaveConsultationRoomWithConfirmation();
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }

    private void LeaveConsultationRoomWithConfirmation()
    {
        ClickElement(LeaveButtonDesktop);
        ClickElement(ConfirmLeaveButton);
    }

    public void InviteParticipant(string displayName)
    {
        var inviteButton = InviteParticipantButton(displayName);
        ClickElement(inviteButton);
    }
    
    public bool CanInviteParticipant(string displayName)
    {
        var inviteButton = InviteParticipantButton(displayName);
        return IsElementVisible(inviteButton);
    }

    /// <summary>
    /// Check if the user has connected to consultation room
    /// </summary>
    /// <param name="displayName">The display name of the user to verify has connected</param>
    /// <returns>true if the user is connected, else false</returns>
    public bool IsParticipantConnected(string displayName)
    {
        var connectedStatus = By.XPath($"//span[normalize-space()='{displayName}'][contains(@class,'yellow')]");
        try
        {
            WaitForElementToBeVisible(connectedStatus);
            return true;
        } catch (Exception)
        {
            Driver.TakeScreenshotAndSave(GetType().Name, "Participant not connected to consultation room when expected");
            return false;
        }
    }
}