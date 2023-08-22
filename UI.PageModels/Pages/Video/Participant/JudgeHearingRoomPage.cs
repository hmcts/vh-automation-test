using OpenQA.Selenium;
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
    
    public void AdmitParticipant(string participantDisplayName)
    {
        OpenContextMenu(participantDisplayName);
        ClickElement(ParticipantAdmitButton(participantDisplayName));
        WaitForElementToBeVisible(ParticipantRemoteMuteButton(participantDisplayName), 60);
    }
    
    public void DismissParticipant(string participantDisplayName)
    {
        OpenContextMenu(participantDisplayName);
        ClickElement(ParticipantDismissButton(participantDisplayName));
        WaitForElementToBeVisible(ParticipantAdmitIconButton(participantDisplayName));
    }

    private void OpenContextMenu(string participantDisplayName)
    {
        var contextLocator = ParticipantContextButton(participantDisplayName);
        if (IsElementVisible(contextLocator))
        {
            ClickElement(ParticipantContextButton(participantDisplayName));    
        }
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
    
    
    private By ParticipantContextButton(string participantDisplayName)
    {
        return By.XPath(
            $"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])/../following-sibling::div[position()=5]//img[@alt='Context menu icon']");
    }
    
    private By ParticipantAdmitButton(string participantDisplayName)
    {
        return By.XPath($"(//span[@class='wrap-anywhere'][normalize-space()='{participantDisplayName}'])/../following-sibling::div[position()=5]//a[normalize-space()='Admit participant']");
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