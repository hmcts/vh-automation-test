using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UI.Pages.PageElements;
using UI.Utilities;

namespace UI.Steps.CommonActions
{
    [Binding]
    ///<summary>
    /// Steps class logout/signout from the application
    ///</summary>
    public class LogoffSteps: ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;

        public LogoffSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"I log off")]
        public void ThenILogOff()
            {
            _scenarioContext.UpdatePageName("logout");
            Driver = (IWebDriver)_scenarioContext["driver"];
            if (ExtensionMethods.IsElementVisible(Driver, Header.LinkSignOut, null))
            {
                Driver.FindElement(Header.LinkSignOut).Click();
            }
            else
            {
                Driver.FindElement(Header.SignOut).Click();
            }
        }

        [Then(@"everyone signs out")]
        public void ThenEveryoneSignsOut()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var participant in _hearing.Participant)
            {
                try
                {
                    Driver = GetDriver(participant.Id, _scenarioContext);
                    if (Driver != null)
                    {
                        Driver.FindElement(Header.SignOut).Click();
                    }
                }
                catch 
                { 
                }
            }
        }
    }
}
