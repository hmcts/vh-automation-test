using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
	///<summary>
	///   CourtRulesPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
     public class CourtRulesPage : VhPage
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