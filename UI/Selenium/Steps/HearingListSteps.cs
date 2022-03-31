using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSpecFlow.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UISelenium.Pages;
namespace UI.Steps
{
    [Binding]
    public class HearingListSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public HearingListSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
        }
        [Then(@"all participants have joined the hearing waiting room")]
        public void ThenAllParticipantsHaveJoinedTheHearingWaitingRoom()
        {
            SignAllParticipantsIn();
            _scenarioContext.UpdatePageName("Participant Hearing Waiting Room");
        }

        public void SignAllParticipantsIn()
        {
            foreach(var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"]) 
            {
                Driver = driver.Value;
                ProceedToWaitingRoom(driver.Key.Split('#').FirstOrDefault(), _hearing.Case.CaseNumber);
            }
        }

        public void ProceedToWaitingRoom(string participant, string caseNumber)
        {
            Driver = GetDriver(participant, _scenarioContext);
            _scenarioContext["driver"] = Driver;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
            Driver.RetryClick(ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext,TimeSpan.FromSeconds(Config.DefaultElementWait));
            if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
            {
                Driver.FindElement(ParticipantHearingListPage.ButtonNext).Click();
                Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                Driver.FindElement(ParticipantHearingListPage.SwitchOnButton).Click();
                Driver.FindElement(ParticipantHearingListPage.WatchVideoButton).Click();
                // Assert video is playing
                Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                if (SkipPracticeVideoHearingDemo)
                {
                    string cameraUrl = Driver.Url.Replace("practice-video-hearing", "camera-working");
                    Driver.Navigate().GoToUrl(cameraUrl);
                    Driver.SwitchTo().Alert().Accept();
                }
                else
                {
                    TestFramework.ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                }
                Driver.FindElement(ParticipantHearingListPage.CameraWorkingYes)?.Click();
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                Driver.FindElement(ParticipantHearingListPage.MicrophoneWorkingYes).Click();
                Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                Driver.FindElement(ParticipantHearingListPage.VideoWorkingYes).Click();
                Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                Driver.FindElement(ParticipantHearingListPage.NextButton).Click();
                Driver.FindElement(ParticipantHearingListPage.DeclareCheckbox).Click();
                Driver.FindElement(ParticipantHearingListPage.NextButton).Click();
            }
        }
    }
}