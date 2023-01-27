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
    /// Steps class for Get Ready For The Hearing page
    ///</summary>
    public class GetReadyForTheHearingSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        GetReadyForTheHearingSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"I get ready for the hearing")]
        public void ThenIGetReadyForTheHearing()
        {
            ExtensionMethods.FindElementWithWait(Driver, GetReadyForTheHearingPage.NextButton,_scenarioContext).Click();
               
        }
    }
}