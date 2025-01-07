namespace UI.PageModels.Pages.Video.Participant;

public class ParticipantHearingRoomPage : VhVideoWebPage
{
    private readonly By _toggleAudioMuteBtn = By.XPath("//button[@id='toggle-audio-mute-img-desktop']//fa-icon[@class='ng-fa-icon']");
    private readonly By _toggleHandRaisedBtn = By.XPath("//button[@id='toggle-hand-raised-img-desktop']");

    public ParticipantHearingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    { }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_toggleAudioMuteBtn);
    }
    
    public void RaiseHand()
    {
        if (IsHandRaised()) return;
        ClickElement(_toggleHandRaisedBtn);
        Thread.Sleep(TimeSpan.FromSeconds(5)); // takes a few seconds for the pexip callback to make a roundtrip
    }
    
    public bool IsHandRaised()
    {
        var toggleHandButton = FindElement(_toggleHandRaisedBtn);
        return toggleHandButton!.GetDomAttribute("class").Contains("Yellow");
    }

    public ParticipantWaitingRoomPage TransferToWaitingRoom()
    {
        return new ParticipantWaitingRoomPage(Driver, DefaultWaitTime);
    }
}