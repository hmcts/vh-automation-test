using OpenQA.Selenium.Interactions;

namespace UI.PageModels.Pages.Video.Participant;

public class JudgeHearingRoomPage : CommonWaitingRoomPage
{
    private readonly By _closeHearingButton = By.Id("end-hearing-desktop");
    private readonly By _confirmCloseHearingButton = By.Id("btnConfirmClose");
    private readonly By _pauseHearing = By.Id("pause-hearing-desktop");
    private By _participantHandRaiseIcon(Guid participantId) => By.XPath($"//div[@id='participants-panel-{participantId}-icon-lowerHand']//fa-icon[@class='ng-fa-icon yellow']");

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
        var transferringInText = GetText(By.CssSelector($"#participants-panel-{participantId}-transferring-in-text"));
        if (!transferringInText.Equals("Joining..."))
        {
            Driver.TakeScreenshotAndSave(GetType().Name, "Admit participant failed");
            throw new InvalidOperationException("Participant joining text is not appearing as expected");
        }
        WaitForElementToBeVisible(ParticipantRemoteMuteButton(displayName), 60);
    }

    /// <summary>
    /// Dismiss the participant from the hearing via the context menu
    /// </summary>
    /// <param name="participantDisplayName"></param>
    /// <param name="participantId">The id from Video API (not the booking)</param>
    public void DismissParticipant(string participantDisplayName, string participantId)
    {
        OpenContextMenu(participantDisplayName);
        Driver.TakeScreenshotAndSave(GetType().Name, "Conext menu opened");
        var dismissButton = ParticipantDismissButton(participantId);
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

    public bool IsParticipantInHearing(Guid participantId)
    {
        var participantMicBtn = By.Id($"participants-panel-{participantId}-icon-micLocal");
        return IsElementVisible(participantMicBtn);
    }

    /// <summary>
    /// Check if a participant is in the hearing after being transferred in on start or being admitted.
    /// Please use <see cref="IsParticipantInHearingAlreadyInSession"/> to check if a participant is in a hearing that has already started.
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public bool IsParticipantInHearing(string displayName)
    {
        var element =
            By.XPath(
                $"//span[contains(text(), '{displayName}')]/../following-sibling::*//*[contains(@id, 'icon-micLocal')]");
        return IsElementVisible(element);

    }

    /// <summary>
    /// Check if a participant is in the hearing that has already started.
    /// The participant would join as remote muted, instead of local muted.
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public bool IsParticipantInHearingAlreadyInSession(string displayName)
    {
        var element =
            By.XPath(
                $"//span[contains(text(), '{displayName}')]/../following-sibling::*//*[contains(@id, 'icon-micRemoteMuted')]");
        WaitForElementToBeClickable(element, 5); // if a user takes more than 5 seconds to join, we should fail 
        return IsElementVisible(element);
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
        Driver.TakeScreenshotAndSave(GetType().Name, "Waiting for countdown to complete");
        const int countdown = 30;
        var waitTime = countdown+buffer;
        Thread.Sleep(TimeSpan.FromSeconds(waitTime));
        Driver.TakeScreenshotAndSave(GetType().Name, $"Countdown expected to complete after {waitTime} seconds");
        WaitForElementToBeInvisible(By.XPath("//img[@src='/assets/images/mic_remote_mute.png']"), 10);
    }

    public bool IsParticipantHandRaised(Guid participantId)
    {
        return IsElementVisible(_participantHandRaiseIcon(participantId));
    }

    public void LowerParticipantHand(Guid participantId)
    {
        ClickElement(_participantHandRaiseIcon(participantId));
        Thread.Sleep(TimeSpan.FromSeconds(5));
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
    
    private By ParticipantDismissButton(string participantId)
    {
        return By.CssSelector($"[id^='judge-context-menu-participant-{participantId}-dismiss-']");
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