
namespace UI.PageModels.Pages.Video.Participant;

public class PanelMemberHearingListPage : VhVideoWebPage

{ 
    private readonly By _checkEquipmentBtn = By.Id("check-equipment-btn");

    public PanelMemberHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    { }

    protected override void ConfirmPageHasLoaded()
        {
            WaitForElementToBeClickable(_checkEquipmentBtn);
        }
        
        public PanelMemberWaitingRoomPage SelectHearing (Guid conferenceId )
        {
            var selectHearingLocator = By.XPath($"//button[contains(@id,'{conferenceId}')]");
            WaitForElementToBeVisible(selectHearingLocator);
            ClickElement(selectHearingLocator);
            return new PanelMemberWaitingRoomPage(Driver, DefaultWaitTime);
        }
}