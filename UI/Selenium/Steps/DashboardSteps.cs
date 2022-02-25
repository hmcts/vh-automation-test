using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSpecFlow.Utilities;
using System;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UISelenium.Pages;
using System.Linq;
using System.Collections.Generic;
namespace UI.Steps
{
    [Binding]
    public class DashboardSteps: ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        
        public DashboardSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I select book a hearing")]
        public void GivenISelectBookAHearing()
        {
            SelectDashboardOption("Book a video hearing");
        }

        public void SelectDashboardOption(string optionName)
        {
            var isPageLoaded = false;
            switch(optionName)
            {
                case "Book a video hearing":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.BookHearingButton).Click();
                    isPageLoaded=IsHeardingDetailsPageLoaded();
                    break;
            }
            isPageLoaded.Should().BeTrue($"cannot load {optionName} page");
        }

        public bool IsHeardingDetailsPageLoaded()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait)));
                wait.Until(ExpectedConditions.ElementIsVisible(HearingDetailsPage.CaseNumber));
               
                return true;
            }
            catch
            {
                //log the exception
                return false;
            }
        }
    }
}
