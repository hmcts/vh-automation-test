using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using System.Linq;
using System.Collections.Generic;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Participant Hearing List Page
    ///</summary>
    public class ParticipantHearingListSteps: ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;        
        public ParticipantHearingListSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I select Check Equipment")]
        public void GivenISelectCheckEquipment()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
            wait.Until(ExpectedConditions.ElementIsVisible(ParticipantHearingListPage.CheckEquipment));
            var element = Driver.FindElement(ParticipantHearingListPage.CheckEquipment);
            element.Click();
        }

        public void SignIntoHearing()
        {
        }
    }
}
