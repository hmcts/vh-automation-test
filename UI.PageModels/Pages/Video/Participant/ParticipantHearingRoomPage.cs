namespace UI.PageModels.Pages.Video.Participant;

public class ParticipantHearingRoomPage : VhVideoWebPage
{
    private readonly By _toggleAudioMuteBtn = By.XPath("//button[@id='toggle-audio-mute-img-desktop']//fa-icon[@class='ng-fa-icon']");

    public ParticipantHearingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    { }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_toggleAudioMuteBtn);
    }

    public ParticipantWaitingRoomPage TransferToWaitingRoom()
    {
        return new ParticipantWaitingRoomPage(Driver, DefaultWaitTime);
    }
}