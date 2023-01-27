using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using UI.Pages;
using UI.Steps.CommonActions;

namespace UI.Utilities
{
    [Binding]
    ///<summary>
    /// Base class that all other Steps class inherit from
    /// Holds Drivers Collection
    ///</summary>
    public class ObjectFactory
    {
        public LoginPage LoginPage { get; set; }
        public CommonPageActions CommonPageActions { get; set; }
        public DashboardPage DashboardPage { get; set; }
        public Dictionary<string, IWebDriver> drivers = new Dictionary<string, IWebDriver>();
        public HearingAssignJudgePage HearingAssignJudgePage { get; set; }
        public EnvironmentConfigSettings Config { get; set; }
        public IWebDriver Driver { get; set; }
        public bool SkipPracticeVideoHearingDemo = true;
        ScenarioContext _scenarioContext;

        public ObjectFactory(ScenarioContext context)
        {
            CommonPageActions = new CommonPageActions((IWebDriver)context["driver"]);
            Config = (EnvironmentConfigSettings)context["config"];
            Driver = (IWebDriver)context["driver"];
            _scenarioContext = context;
        }

        public IWebDriver GetDriver(string participant, ScenarioContext _scenarioContext)
        {
            var driver = ((Dictionary<string, IWebDriver>)_scenarioContext["drivers"]).Where(a => a.Key.ToLower().Contains(participant.ToLower()))?.FirstOrDefault().Value;
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                _scenarioContext["driver"] = driver;
            }
            catch
            {
                var key = ((Dictionary<string, IWebDriver>)_scenarioContext["drivers"]).Where(a => a.Key.ToLower().Contains(participant.ToLower()))?.FirstOrDefault().Key;
                ((Dictionary<string, IWebDriver>)_scenarioContext["drivers"]).Remove(key);
                return null;
            }
            return driver;
        }

        public IWebDriver StartNewDriver()
        {
            Driver?.Dispose();
            this.Driver = new DriverFactory().InitializeDriver(Config.BrowserType);
            _scenarioContext["driver"] = this.Driver;
            return Driver;
        }

        public void SwitchToWindowByTitle(string title)
        {
            foreach (var w in Driver.WindowHandles)
            {
                var f = Driver.SwitchTo().Window(w).Title;
                if(f == title)
                {
                    break;
                }
            }
        }
    }
}
