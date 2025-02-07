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
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_checkEquipmentBtn);
    }

    public GetReadyForTheHearingIntroductionPage SelectHearing(string caseNumber)
    {
        var selectHearingLocator = By.XPath($"//button[contains(@aria-label,'{caseNumber}')]");
        WaitForElementToBeVisible(selectHearingLocator, 60);
        ClickElement(selectHearingLocator);
        return new GetReadyForTheHearingIntroductionPage(Driver, DefaultWaitTime);
    }
    
    public GetReadyForTheHearingIntroductionPage SelectHearing(Guid conferenceId)
    {
        var selectHearingLocator = By.XPath($"//button[contains(@id, '{conferenceId}')]");
        WaitForElementToBeVisible(selectHearingLocator, 60);
        ClickElement(selectHearingLocator);
        return new GetReadyForTheHearingIntroductionPage(Driver, DefaultWaitTime);
    }
}