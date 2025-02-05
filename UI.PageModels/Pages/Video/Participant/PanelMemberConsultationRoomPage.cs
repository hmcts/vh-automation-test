namespace UI.PageModels.Pages.Video.Participant;

public class PanelMemberConsultationRoomPage :VhVideoWebPage
{
    public PanelMemberConsultationRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime){}
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(LeaveButtonDesktop);
    }
    private static By LeaveButtonDesktop => By.Id("leaveButton-desktop");
    
    public PanelMemberWaitingRoomPage LeavePanelMemberConsultationRoom()
    {
        LeavePanelMemberConsultationRoom();
        return new PanelMemberWaitingRoomPage(Driver, DefaultWaitTime);
    }
    
}