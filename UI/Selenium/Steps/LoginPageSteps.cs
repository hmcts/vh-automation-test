using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    ///<summary>
    /// Steps class for Login page
    ///</summary>
    [Binding]
    public class LoginPageSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public string LoginUrl { get; set; }
        public string UserName;
        public LoginPageSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            LoginUrl = Config.AdminUrl;
        }

        [Given(@"I log in video url as ""([^""]*)""")]
        public void GivenILogInVideoUrlAs(string userName)
        {
            LoginByUrl(userName, Config.VideoUrl);
        }

        [Given(@"I log in hearing url ""([^""]*)""")]
        public void GivenILogInHearingUrl(string userName)
        {
            var result = CommonPageActions.NavigateToPage(LoginUrl, "login.microsoftonline.com");
        }

        [Given(@"I log in as ""([^""]*)""")]
        public void GivenILogInAs(string userName)
        {
            LoginByUrl(userName, LoginUrl);
        }

        private void LoginByUrl(string userName, string url)
        {
            _scenarioContext.UpdatePageName("Login");
            var result= CommonPageActions.NavigateToPage(url, "login.microsoftonline.com");
            Login(userName, Config.UserPassword);
            _scenarioContext.UpdateUserName(userName);
            _scenarioContext.UpdatePageName("Dashboard");
        }

        public void Login(string username, string password)
        {
            ExtensionMethods.FindElementWithWait(Driver, LoginPage.UsernameTextfield, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).SendKeys(username);
            Driver.FindElement(LoginPage.Next).Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
            wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.PasswordField));
            wait.Until(ExpectedConditions.ElementToBeClickable(LoginPage.SignIn));
            wait.Until(ExpectedConditions.ElementToBeClickable(LoginPage.BackButton));
            Driver.FindElement(LoginPage.PasswordField).SendKeys(password);
            ExtensionMethods.FindElementWithWait(Driver, LoginPage.SignIn, _scenarioContext).Click();
            ExtensionMethods.CheckForUnExpectedErrors(Driver);
        }

        [Then(@"all participants log in to video web")]
        public void ThenAllParticipantsLogInToVideoWeb()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            Driver?.Quit();
            foreach (var participant in _hearing.Participant)
            {
                Driver = new DriverFactory().InitializeDriver(Config.BrowserType);
                ((List<int>)_scenarioContext["ProcessIds"]).AddRange(DriverFactory.ProcessIds);
                ((Dictionary<string, IWebDriver>)_scenarioContext["drivers"]).Add($"{participant.Id}#{participant.Party.Name}-{participant.Role.Name}", Driver);
                Driver = GetDriver(participant.Id, _scenarioContext);
                Driver.Navigate().GoToUrl(Config.VideoUrl);
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
                wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.UsernameTextfield));
                _scenarioContext.UpdatePageName("Video Web Login");
                Login(participant.Id, Config.UserPassword);
            }
            _scenarioContext.UpdatePageName("Your Video Hearings");
        }

        [Given(@"I open a new browser and log into admin web as ""([^""]*)""")] 
        public void GivenIOpenANewBrowserAndLogInAs(string email)
        {
            Driver = new DriverFactory().InitializeDriver(Config.BrowserType);
            ((List<int>)_scenarioContext["ProcessIds"]).AddRange(DriverFactory.ProcessIds);
            ((Dictionary<string, IWebDriver>)_scenarioContext["drivers"]).Add(email, Driver);
            Driver = GetDriver(email, _scenarioContext);
            Driver.Navigate().GoToUrl(Config.AdminUrl);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
            wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.UsernameTextfield));
            Login(email, Config.UserPassword);
        }

        [When(@"Video Hearing Officer logs into video web as ""([^""]*)""")]
        public void WhenVideoHearingOfficerLogsIntoVideoWebAs(string email)
        {
            Driver = new DriverFactory().InitializeDriver(Config.BrowserType);
            _scenarioContext["driver"]=Driver;
            ((List<int>)_scenarioContext["ProcessIds"]).AddRange(DriverFactory.ProcessIds);
            ((Dictionary<string, IWebDriver>)_scenarioContext["drivers"]).Add(email, Driver);
            Driver = GetDriver(email, _scenarioContext);
            Driver.Navigate().GoToUrl(Config.VideoUrl);
            _hearing.Participant.Add(new Participant
            {
                Id = email,
                Party = new Party
                {
                    Name = "VHO"
                },
                Role = new Role
                {
                    Name = "VHO"
                }
            });
            var participant = _hearing.Participant.FirstOrDefault(a => a.Id == email);
            drivers.Add($"{participant.Id}#{participant.Party.Name}-{participant.Role.Name}", Driver);
            Login(participant.Id, Config.UserPassword);
        }
    }
}