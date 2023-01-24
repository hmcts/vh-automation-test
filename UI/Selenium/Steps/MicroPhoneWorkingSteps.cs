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
    /// Steps class for Microphone Working page
    ///</summary>
    public class MicroPhoneWorkingSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        MicroPhoneWorkingSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"Checking was your microphone working")]
        public void ThenCheckingWasYourMicrophoneWorking()
        {
            ExtensionMethods.FindElementWithWait(Driver, MicroPhoneWorkingPage.MicrophoneYesRadioBUtton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, MicroPhoneWorkingPage.Continue, _scenarioContext).Click();
        }
    }
}