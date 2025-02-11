using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class CourtRulesPage : VhVideoWebPage
{
    private readonly By _courtRulesContinueBtn = By.Id("nextButton");

    public CourtRulesPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeVisible(By.XPath("//h1[contains(text(), 'Court rules')]"));
    }

    public void ConfirmPanelMemberPageHasLoaded()

    {
        WaitForElementToBeVisible(locator:By.XPath("//h1[contains(text(), 'Court rules')]"));
    }
    public DeclarationPage AcceptCourtRules()
    {
        ConfirmPageHasLoaded();
        ClickElement(_courtRulesContinueBtn);
        return new DeclarationPage(Driver, DefaultWaitTime);
    }
}