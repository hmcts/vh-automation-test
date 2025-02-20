﻿namespace UI.PageModels.Pages.Video.Participant;

public class ConsultationRoomPage : VhVideoWebPage

{
    public ConsultationRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(LeaveButtonDesktop);
    }

    public static By InviteParticipants => By.ClassName("phone");
    public static By ParticipantsTick => By.CssSelector(".member-group+.member-group .yellow fa-icon");
    public static By ConfirmLeaveButton => By.Id("consultation-leave-button");
    public static By CloseButton => By.Id("closeButton");
    public static By LeaveButtonDesktop => By.Id("leaveButton-desktop");
    public static By WaitingRoomIframe => By.Id("admin-frame");
    public static By WaitingRoomJudgeLink => By.XPath("//table[@id='WaitingRoom']//td[contains(.,'Judge')]");

    public static By WaitingRoomParticipantLink =>
        By.XPath("//td[contains(@id,'-WaitingRoom-menu')][not(text(),'Judge')]");

    public static By WaitingRoomMenu => By.XPath("//ul[contains(@id,'-WaitingRoom-menu')]");
    public static By PrivateConsultation => By.XPath("//ul[contains(@id,'-WaitingRoom-menu')]//li");
    public static By SelfViewButton => By.Id("selfViewButton");
    public static By MuteButton => By.Id("muteButton");

    public static By InviteParticipant(string name) =>
        By.XPath($"//div[@class='participant-endpoint-row' and contains(.,'{name}')]//app-invite-participant");

    public static By ParticipantDisplayName(string name) =>
        By.XPath($"//div[@class='participant-endpoint-row' and contains(.,'{name}')]");

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