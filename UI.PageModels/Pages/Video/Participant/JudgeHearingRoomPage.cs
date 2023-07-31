using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class JudgeHearingRoomPage : VhVideoWebPage
{
    private readonly By _closeHearingButton = By.Id("end-hearing-desktop");
    private readonly By _confirmCloseHearingButton = By.Id("btnConfirmClose");
    private readonly By _pauseHearing = By.Id("pause-hearing-desktop");

    public JudgeHearingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeInvisible(_pauseHearing, DefaultWaitTime);
    }

    public JudgeWaitingRoomPage CloseHearing()
    {
        ClickElement(_closeHearingButton);
        ClickElement(_confirmCloseHearingButton);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }
    
    public void AdmitParticipant(string participantDisplayName)
    {
        var admitButton = By.Id($"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])[1]/../following-sibling::div[2]//fa-icon");
        ClickElement(admitButton);
    }

    public bool IsParticipantInHearing(string participantDisplayName)
    {
        var participantMicBtn =
            By.XPath(
                $"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])[1]/../following-sibling::div[2]//fa-icon[@icon='microphone']");
        return IsElementVisible(participantMicBtn);
    }
    
    public JudgeWaitingRoomPage PauseHearing()
    {
        ClickElement(_pauseHearing);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }
}

public class ParticipantHearingRoomPage : VhVideoWebPage
{
    private readonly By _toggleAudioMuteBtn = By.XPath("//div[@id='toggle-audio-mute-img-desktop']//fa-icon[@class='ng-fa-icon']");

    public ParticipantHearingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeVisible(_toggleAudioMuteBtn);
    }

    public ParticipantWaitingRoomPage TransferToWaitingRoom()
    {
        return new ParticipantWaitingRoomPage(Driver, DefaultWaitTime);
    }
}