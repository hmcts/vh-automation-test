namespace UI.PageModels.Pages.Video.Participant;

/// <summary>
///     This page represents the 'introduction' page for the participant after selecting a hearing
/// </summary>
public class GetReadyForTheHearingIntroductionPage(IWebDriver driver, int defaultWaitTime)
    : VhVideoWebPage(driver, defaultWaitTime)
{
    private readonly By _nextButton = By.Id("next");
    private readonly By _skipButton = By.Id("skipToCourtRulesBtn");

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_nextButton, 60); // cold start sometimes takes a while
    }
    
    public CourtRulesPage SkipToCourtRules()
    {
        ClickElement(_skipButton);
        return new CourtRulesPage(Driver, DefaultWaitTime);
    }
    
    public bool IsSkipButtonVisible()
    {
        try
        {
            WaitForElementToBeVisible(_skipButton, 5);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public CheckEquipmentPage GoToEquipmentCheck()
    {
        ClickElement(_nextButton);
        return new CheckEquipmentPage(Driver, DefaultWaitTime);
    }
}