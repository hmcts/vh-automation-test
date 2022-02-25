
using SeleniumSpecFlow.Utilities;
using TechTalk.SpecFlow;
using UI.Model;
using UISelenium.Pages;
using FluentAssertions;
namespace UI.Steps
{
    [Binding]
    public class LogoffSteps: ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        private static string loginUrl = "login.microsoftonline.com";

        public LogoffSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
        }

        [Then(@"I log off")]
        public void ThenILogOff()
        {
            if (Driver.FindElement(Header.LinkSignOut).Displayed)
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
            foreach (var participant in _hearing.Participant)
            {
                Driver = GetDriver(participant.Id, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                ThenILogOff();
                Driver.Url.Should().Contain(loginUrl);
            }

            Driver?.Close();
        }
    }
}
