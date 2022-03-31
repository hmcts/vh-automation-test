using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSpecFlow.Utilities;
using System;
using TechTalk.SpecFlow;
using TestFramework;
using UISelenium.Pages;
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
            _scenarioContext.UpdatePageName("Book a video hearing");
        }

        public void SelectDashboardOption(string optionName)
        {
            var isPageLoaded = false;
            switch(optionName)
            {
                case "Book a video hearing":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.BookHearingButton, _scenarioContext).Click();
                    isPageLoaded=IsHeardingDetailsPageLoaded();
                    isPageLoaded.Should().BeTrue($"cannot load {optionName} page");
                    break;
                case "Get audio file link":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.GetAudioFileLinkButton, _scenarioContext).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, GetAudioFilePage.CaseNumberInput);
                    break;
            }
        }

        public bool IsHeardingDetailsPageLoaded()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
                wait.Until(ExpectedConditions.ElementIsVisible(HearingDetailsPage.CaseNumber));
               
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
