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
    
    public JudgeWaitingRoomPage PauseHearing()
    {
        ClickElement(_pauseHearing);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }

    public void ConfirmParticipantConnected(string participantUsername)
    {
        throw new NotImplementedException();
    }
}