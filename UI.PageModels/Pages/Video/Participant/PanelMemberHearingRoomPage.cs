using OpenQA.Selenium.Interactions;
namespace UI.PageModels.Pages.Video.Participant;

public class PanelMemberHearingRoomPage(IWebDriver driver, int defaultWaitTime)
    : VhVideoWebPage(driver, defaultWaitTime)
{
    private readonly By _toggleAudioMuteBtn = By.XPath("//button[@id='toggle-audio-mute-img-desktop']//fa-icon[@class='ng-fa-icon']");
    private readonly By _toggleHandRaisedBtn = By.XPath("//button[@id='toggle-hand-raised-img-desktop']");

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
    
    private bool IsHandRaised()
    {
        var toggleHandButton = FindElement(_toggleHandRaisedBtn);
        return toggleHandButton!.GetDomAttribute("class").Contains("Yellow");
    }
    
}



