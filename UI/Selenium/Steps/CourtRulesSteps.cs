using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using TestFramework;
using OpenQA.Selenium.Interactions;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Court Rules Page
    ///</summary>
    public class CourtRulesSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
		
        CourtRulesSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"I agree to court rules")]
        public void ThenIAgreeToCourtRules()
        {
            ExtensionMethods.FindElementWithWait(Driver, CourtRulesPage.CourtRulesContinueBtn, _scenarioContext).Click();
        }
    }
}