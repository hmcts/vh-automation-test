using UI.PageModels.Pages.Video.Participant;

namespace UI.PageModels.Pages.Video.QuickLink;

/// <summary>
///     The Quick Links hearing list page
/// </summary>
public class QuickLinkHearingListPage : VhVideoWebPage
{
    private readonly By _checkEquipmentButton = By.Id("check-equipment-btn");
    private readonly By _signInToHearingButton = By.XPath("//button[contains(@id,'sign-into-hearing-btn-')]");
    
    public QuickLinkHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    public GetReadyForTheHearingIntroductionPage SelectHearing(string caseName)
    {
        var selectHearingLocator = By.XPath($"//tr[contains(.,'{caseName}')]//button");
        ClickElement(selectHearingLocator);
        return new GetReadyForTheHearingIntroductionPage(Driver, DefaultWaitTime);
    }
}