namespace UI.PageModels.Pages.Video.Participant;

public class JudgeHearingListPage : VhVideoWebPage
{
    private readonly By _checkEquipmentBtn = By.Id("check-equipment-btn");

    public JudgeHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    { }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_checkEquipmentBtn);
    }

    public JudgeWaitingRoomPage SelectHearing(string caseNumber)
    {
        var selectHearingLocator = By.XPath($"//button[contains(@aria-label,'{caseNumber}')]");
        return SelectHearing(selectHearingLocator);
    }
    
    public JudgeWaitingRoomPage SelectHearing(Guid conferenceId)
    {
        var selectHearingLocator = By.XPath($"//button[contains(@id,'{conferenceId}')]");
        return SelectHearing(selectHearingLocator);
    }

    private JudgeWaitingRoomPage SelectHearing(By locator)
    {
        ClickElement(locator);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }
}