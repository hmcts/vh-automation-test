using OpenQA.Selenium.Interactions;

namespace UI.PageModels.Pages.Video.Participant;

public class JudgeHearingRoomPage : VhVideoWebPage
{
    private readonly By _closeHearingButton = By.Id("end-hearing-desktop");
    private readonly By _confirmCloseHearingButton = By.Id("btnConfirmClose");
    private readonly By _pauseHearing = By.Id("pause-hearing-desktop");

    public JudgeHearingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    { }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeVisible(_pauseHearing, DefaultWaitTime);
    }

    public JudgeWaitingRoomPage CloseHearing()
    {
        ClickElement(_closeHearingButton);
        ClickElement(_confirmCloseHearingButton);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }
    
    public void AdmitParticipant(string displayName, string participantId)
    {
        var admitButton = ParticipantAdmitButton(participantId);
        WaitForElementToBeVisible(admitButton);
        ClickElement(admitButton);
        WaitForElementToBeVisible(ParticipantRemoteMuteButton(displayName), 60);
    }
    
    public void DismissParticipant(string participantDisplayName)
    {
        OpenContextMenu(participantDisplayName);
        var dismissButton = ParticipantDismissButton(participantDisplayName);
        WaitForElementToBeVisible(dismissButton);
        ClickElement(dismissButton);
        WaitForElementToBeVisible(ParticipantAdmitIconButton(participantDisplayName));
    }

    private void OpenContextMenu(string participantDisplayName)
    {
        var contextLocator = ParticipantContextButton(participantDisplayName);
        if (IsElementVisible(contextLocator))
            ClickElement(ParticipantContextButton(participantDisplayName));    
        else
        {
            var elem = Driver.FindElement(contextLocator);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView();", elem);
            new Actions(Driver).ScrollToElement(Driver.FindElement(contextLocator)).Perform();
            ClickElement(ParticipantContextButton(participantDisplayName));
        }
        
        // wait for context menu to be visible
        WaitForElementToBeVisible(By.XPath($"//app-judge-context-menu//strong[text()='{participantDisplayName}']"));
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

    /// <summary>
    /// Wait for the hearing countdown to complete
    /// </summary>
    /// <param name="buffer">Additional buffer, in seconds</param>
    public void WaitForCountdownToComplete(int buffer = 5)
    {
        const int countdown = 30;
        Thread.Sleep(TimeSpan.FromSeconds(countdown + buffer));
    }
    
    private By ParticipantContextButton(string participantDisplayName)
    {
        return By.XPath(
            $"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])/../following-sibling::div[position()=5]//img[@alt='Context menu icon']");
    }
    
    private By ParticipantAdmitFromContextButton(string participantId)
    {
        return By.XPath($"//a[contains(@id, 'judge-context-menu-participant-{participantId}')]");
    }
    
    private By ParticipantAdmitButton(string participantId)
    {
        return By.Id($"participants-panel-{participantId}-admit-participant-icon");
    }
    
    private By ParticipantDismissButton(string participantDisplayName)
    {
        return By.XPath($"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])/../following-sibling::div[position()=5]//a[normalize-space()='Dismiss participant']");
    }
    
    private By ParticipantRemoteMuteButton(string participantDisplayName)
    {
        return By.XPath($"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])/../following-sibling::div[position()=2]//fa-icon[@icon='microphone']");
    }
    
    private By ParticipantAdmitIconButton(string participantDisplayName)
    {
        return By.XPath($"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])/../following-sibling::div[position()=2]//fa-icon[@icon='sign-in-alt']");
    }
}