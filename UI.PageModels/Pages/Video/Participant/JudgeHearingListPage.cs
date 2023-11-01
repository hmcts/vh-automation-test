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

    public JudgeWaitingRoomPage SelectHearing(Guid conferenceId)
    {
        var selectHearingLocator = By.XPath($"//button[contains(@id,'{conferenceId}')]");
        ClickElement(selectHearingLocator);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }
}