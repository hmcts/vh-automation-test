namespace UI.PageModels.Pages.Video.Participant;

public class PanelMemberWaitingRoomPage(IWebDriver driver, int defaultWaitTime)  : VhVideoWebPage(driver, defaultWaitTime)
{

    private readonly By _enterPanelMemberConsultationRoomBtn = By.Id("btnStartConcultation");
    
    protected override void ConfirmPageHasLoaded()
    {
        // the start/resume hearing button is not available when a hearing is closed
        WaitForElementToBeClickable(_enterPanelMemberConsultationRoomBtn, DefaultWaitTime);
    }

    public PanelMemberHearingRoomPage EnterConsultationRoom()
    {
        ClickElement(_enterPanelMemberConsultationRoomBtn);
        Driver.TakeScreenshotAndSave(GetType().Name, "Prompted to enter the consultation");
        ClickElement(_enterPanelMemberConsultationRoomBtn);
        Driver.TakeScreenshotAndSave(GetType().Name, "enter to consultation");
        return new PanelMemberHearingRoomPage(Driver, DefaultWaitTime);

    }
   public ConsultationRoomPage JoinConsultationRoomPage()

    {
        ClickElement(_enterPanelMemberConsultationRoomBtn);
        return new ConsultationRoomPage (Driver, DefaultWaitTime);
    }
}


    
