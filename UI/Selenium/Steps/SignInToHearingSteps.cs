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
    /// Steps class for Sign into Hearing
    ///</summary>
    public class SignInToHearingSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        SignInToHearingSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
                
        [Then(@"I click on signintoHearing")]
        public void ThenIClickOnSignintoHearing()
        {
            ExtensionMethods.FindElementWithWait(Driver, SignInToHearingPage.SignInToHearingButton, _scenarioContext).Click();           
        }        
    }
}