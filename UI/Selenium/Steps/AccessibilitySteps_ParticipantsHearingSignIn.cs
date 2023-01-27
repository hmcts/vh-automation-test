using TechTalk.SpecFlow;
using UI.Model;
using Selenium.Axe;
using TestFramework;
using OpenQA.Selenium;
using FluentAssertions;
using System.Linq;
using UI.Pages;
using UI.Steps.CommonActions;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for checking accessibility for Participants Hearing Sign In
    ///</summary>
    public class AccessibilitySteps_ParticipantsHearingSignIn : ObjectFactory
    {
        private ScenarioContext _scenarioContext;
        private LoginPageSteps loginSteps;
        private DashboardSteps dashboardSteps;
        private AxeBuilder axeResult;
        AccessibilitySteps_VideoBooking accessibilitySteps;
        BookingListSteps bookingListSteps;
        public string username = "auto_vw.individual_05@hearings.reform.hmcts.net";
        string _pageName;
        Hearing _hearing;
        LogoffSteps logoffSteps;

        public AccessibilitySteps_ParticipantsHearingSignIn(ScenarioContext scenarioContext)
           : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            loginSteps = new LoginPageSteps(scenarioContext);
            dashboardSteps = new DashboardSteps(scenarioContext);
            bookingListSteps = new BookingListSteps(scenarioContext);
            accessibilitySteps = new AccessibilitySteps_VideoBooking(scenarioContext);
            logoffSteps = new LogoffSteps(scenarioContext);
            axeResult = new AxeBuilder(Driver);
        }

        [Given(@"an individual on the ""([^""]*)"" page")]
        public void GivenAnIndividualOnThePage(string pageName)
        {
            _pageName = pageName;
            accessibilitySteps.PageName = pageName;
            if (string.IsNullOrEmpty((string)_scenarioContext["AccessibilityBaseUrl"]))
            {
                bookingListSteps.GivenIHaveABookedHearingInNextMinutes(3);
                _hearing = (Hearing)_scenarioContext["Hearing"];
            }            
            loginSteps.LoginUrl = Config.VideoUrl;
        }

        [Then(@"assert page should be accessible")]
        public void ThenAssertPageShouldBeAccessible()
        {
            LoginAsParticipant();
            CheckAccessibility(_pageName);
        }

        public void CheckAccessibility(string pageName)
        {
            pageName = pageName.ToLower();
            if (pageName == "hearing-list")
            {
                Driver.Navigate().GoToUrl(loginSteps.LoginUrl + "/participant/hearing-list");
                axeResult = new AxeBuilder((IWebDriver)_scenarioContext["driver"]);
                axeResult.Analyze().Violations.Where(e => e.Impact != "minor").Should().BeEmpty();
            }
            else
            {
                if (string.IsNullOrEmpty((string)_scenarioContext["AccessibilityBaseUrl"]))
                {
                    Driver.Navigate().GoToUrl(loginSteps.LoginUrl + "/participant/hearing-list");
                    ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber)).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.ButtonNext);
                    _scenarioContext["AccessibilityBaseUrl"] = Driver.Url;
                }
                if(Driver.Url != (string)_scenarioContext["AccessibilityBaseUrl"])
                {
                    Driver.Navigate().GoToUrl((string)_scenarioContext["AccessibilityBaseUrl"]);
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.ButtonNext);
                    ExtensionMethods.AcceptAlert(Driver);
                }
                Driver.Navigate().GoToUrl(Driver.Url.Replace("introduction", pageName));
                ExtensionMethods.AcceptAlert(Driver);
                System.Threading.Thread.Sleep(1000); // AXE FAILS IF SLEEP IS REMOVED coz ANALYSIS MAY HAPEN BEFORE PAGE'S FULLY LOADED
                axeResult = new AxeBuilder((IWebDriver)_scenarioContext["driver"]);
                axeResult.Analyze().Violations.Where(e => e.Impact != "minor").Should().BeEmpty();
            }
        }

        private void ProceedToPage(string pageName)
        {
            CheckAccessibility(pageName);
        }

        private void LoginAsParticipant()
        {
            this.Driver = StartNewDriver();
            _scenarioContext["driver"] = this.Driver;
            loginSteps = new LoginPageSteps(_scenarioContext);
            axeResult = new AxeBuilder(Driver);
            dashboardSteps = new DashboardSteps(_scenarioContext);
            loginSteps.LoginUrl = Config.VideoUrl;
            loginSteps.UserName = username;
            loginSteps.GivenILogInAs(username);
        }

        public void RetryBookingConfirmationIfFails()
        {
            if(ExtensionMethods.IsElementVisible(Driver, BookingDetailsPage.CloseBookingFailureWindowButton, _scenarioContext))
            {
                ExtensionMethods.FindElementEnabledWithWait(Driver, BookingDetailsPage.CloseBookingFailureWindowButton).Click();
            }
        }
    }
}
