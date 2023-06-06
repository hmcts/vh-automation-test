using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
     public class CourtRulesPage : VhVideoWebPage
    {
       private readonly By _courtRulesContinueBtn = By.Id("nextButton");

       public CourtRulesPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
       {
       }
       
       public DeclarationPage AcceptCourtRules()
	   {
		   ClickElement(_courtRulesContinueBtn);
		   return new DeclarationPage(Driver, DefaultWaitTime);
	   }
    }
}