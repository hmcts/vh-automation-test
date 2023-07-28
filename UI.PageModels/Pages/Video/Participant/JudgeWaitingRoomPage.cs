using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class JudgeWaitingRoomPage : VhVideoWebPage
{
    private readonly By _confirmStartHearingButton = By.Id("btnConfirmStart");

    private readonly By _startOrResumeHearingBtn =
        By.XPath("//button[normalize-space()='Start video hearing' or normalize-space()='Resume video hearing']");
    
    private readonly By _enterConsultationRoomBtn = By.Id("joinPCButton");

    public JudgeWaitingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        // the start/resume hearing button is not available when a hearing is closed
        WaitForElementToBeClickable(_enterConsultationRoomBtn);
    }

    public JudgeHearingRoomPage StartOrResumeHearing()
    {
        ClickElement(_startOrResumeHearingBtn);
        ClickElement(_confirmStartHearingButton);
        return new JudgeHearingRoomPage(Driver, DefaultWaitTime);
    }

    public bool IsHearingClosed()
    {
        return IsElementVisible(By.XPath("//h1[normalize-space()='Hearing is closed']"));
    }

    public string GetParticipantStatus(string fullName)
    {
        return GetText(By.XPath($"//dt[normalize-space()='{fullName}']/following-sibling::dd[2]//label"));
    }

    public string GetVideoAccessPointStatus(string accessPointName)
    {
        return GetText(By.XPath($"//dt[normalize-space()='{accessPointName}']/following-sibling::dd//label"));
    }

    public int GetParticipantConnectedCount()
    {
        var connectedLabel = Driver.FindElements(By.XPath("//label[normalize-space()='Connected']"));
        return connectedLabel.Count;
    }
}