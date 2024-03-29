namespace UI.PageModels.Pages.Video.Participant;

public class SeeAndHearVideoPage : VhVideoWebPage
{
    private readonly By _continueBtn = By.Id("continue-btn");
    private readonly By _yesRadioButton = By.XPath("//label[normalize-space()='Yes']");

    public SeeAndHearVideoPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_yesRadioButton);
    }

    public CourtRulesPage SelectYesToVisualAndAudioClarity()
    {
        ConfirmPageHasLoaded();
        ClickElement(_yesRadioButton);
        ClickElement(_continueBtn);
        return new CourtRulesPage(Driver, DefaultWaitTime);
    }
}