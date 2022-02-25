using TechTalk.SpecFlow;
using SeleniumSpecFlow.Utilities;
using UISelenium.Pages;
using FluentAssertions;
using SeleniumExtras.WaitHelpers;
using SeleniumExtras;
using OpenQA.Selenium.Support.UI;
using System;
using UI.Model;
using TestLibrary.Utilities;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace SeleniumSpecFlow.Steps
{
    [Binding]
    public class LoginPageSteps : ObjectFactory
    {
        private ScenarioContext _scenarioContext;
        private Hearing _hearing;
        private Dictionary<string, IWebDriver> drivers = new Dictionary<string, IWebDriver>();
        public LoginPageSteps(ScenarioContext scenarioContext)
            :base (scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I log in as ""([^""]*)""")]
        public void GivenILogInAs(string userName)
        {
            var result= CommonPageActions.NavigateToPage(Config.URL, "login.microsoftonline.com");
            Login(userName, Config.BambooPassword);
        }
    
        public void Login(string username, string password)
        {
            TestFramework.ExtensionMethods.FindElementWithWait(Driver, LoginPage.UsernameTextfield, TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait))).SendKeys(username);
            Driver.FindElement(LoginPage.Next).Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait)));
            wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.PasswordField));
            wait.Until(ExpectedConditions.ElementToBeClickable(LoginPage.SignIn));
            wait.Until(ExpectedConditions.ElementToBeClickable(LoginPage.BackButton));
            Driver.FindElement(LoginPage.PasswordField).SendKeys(password);
            TestFramework.ExtensionMethods.FindElementWithWait(Driver, LoginPage.SignIn).Click();
        }

        [Then(@"all participants log in to video web")]
        public void ThenAllParticipantsLogInToVideoWeb()
        {
            Driver?.Close();
   
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var participant in _hearing.Participant)
            {
                Driver = new DriverFactory().InitializeDriver(TestConfigHelper.browser);
                _scenarioContext["driver"] = Driver;
                Driver.Navigate().GoToUrl(Config.VideoUrl);
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait)));
                wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.UsernameTextfield));
                drivers.Add($"{participant.Id}#{participant.Party.Name}-{participant.Role.Name}", Driver);
                    
                Login(participant.Id, Config.BambooPassword);
            }
            _scenarioContext.Add("drivers", drivers);
        }
    }
}
