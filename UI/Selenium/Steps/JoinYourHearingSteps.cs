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
    /// Steps class for Join Your Hearing Page
    ///</summary>
    public class JoinYourHearingSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        JoinYourHearingSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"I want to join hearing with details")]
        public void ThenIWantToJoinHearingWithDetails(Table table)
        {
                foreach (var row in table.Rows)
            {
                ExtensionMethods.FindElementWithWait(Driver, JoinYourHearingPage.FullName, _scenarioContext).SendKeys(row[0]);
            }
            if(ExtensionMethods.IsElementExists(Driver, JoinYourHearingPage.QuickLinkParticipant, _scenarioContext))
            {
                ExtensionMethods.FindElementWithWait(Driver, JoinYourHearingPage.QuickLinkParticipant, _scenarioContext).Click();      
            }
            if (ExtensionMethods.IsElementExists(Driver, JoinYourHearingPage.ContinueButton, _scenarioContext))
            {
                ExtensionMethods.FindElementWithWait(Driver, JoinYourHearingPage.ContinueButton, _scenarioContext).Click();
            }
        }
    }
}