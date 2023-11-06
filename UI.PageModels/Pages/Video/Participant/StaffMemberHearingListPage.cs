namespace UI.PageModels.Pages.Video.Participant;

public class StaffMemberHearingListPage : VhVideoWebPage
{
    private readonly By _checkEquipmentBtn = By.Id("check-equipment-btn");

    public StaffMemberHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    { }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_checkEquipmentBtn);
    }

    public JudgeWaitingRoomPage SelectHearing(Guid conferenceId)
    {
        var selectHearingLocator = By.XPath($"//button[contains(@id,'{conferenceId}')]");
        ClickElement(selectHearingLocator);
        return new StaffMemberWaitingRoomPage(Driver, DefaultWaitTime);
    }
}