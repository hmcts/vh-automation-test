using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

/// <summary>
///     This page represents the 'introduction' page for the participant after selecting a hearing
/// </summary>
public class GetReadyForTheHearingIntroductionPage : VhVideoWebPage
{
    private readonly By _nextButton = By.Id("next");

    public GetReadyForTheHearingIntroductionPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_nextButton);
    }

    public CheckEquipmentPage GoToEquipmentCheck()
    {
        ClickElement(_nextButton);
        return new CheckEquipmentPage(Driver, DefaultWaitTime);
    }
}