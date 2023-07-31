using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class ParticipantHearingListPage : VhVideoWebPage
{
    public static By HearingListPageTitle =
        By.XPath(
            "//*[contains(text(), 'Video hearings for') or contains(text(),'Your video hearing') or contains(text(),'Your video hearings')]");

    public static By TestingYourEquipment = By.XPath("//*[contains(text(), ' Testing your equipment')]");
    private readonly By _checkEquipmentBtn = By.Id("check-equipment-btn");

    public ParticipantHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_checkEquipmentBtn);
    }
    
    public GetReadyForTheHearingIntroductionPage SelectHearing(string caseName)
    {
        var selectHearingLocator = By.XPath($"//tr[contains(.,'{caseName}')]//button");
        WaitForElementToBeVisible(selectHearingLocator, 60);
        ClickElement(selectHearingLocator);
        return new GetReadyForTheHearingIntroductionPage(Driver, DefaultWaitTime);
    }
}