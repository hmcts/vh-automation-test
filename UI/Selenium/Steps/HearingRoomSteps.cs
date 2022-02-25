using SeleniumSpecFlow.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Model;
using UISelenium.Pages;
using TechTalk.SpecFlow;
using NUnit.Framework;
using FluentAssertions;
using TestFramework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UI.Steps
{
    public class HearingRoomSteps : ObjectFactory
    {
        private ScenarioContext _scenarioContext;
        private Hearing _hearing;

        public HearingRoomSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
        }

        [Then(@"the judge checks that all participants have joined the hearing room")]
        public void ThenTheJudgeChecksThatAllParticipantsHaveJoinedTheHearingRoom()
        {
            Driver = GetDriver("Judge", _scenarioContext);
            _scenarioContext["driver"] = Driver;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
            wait.Until(ExpectedConditions.ElementToBeClickable(HearingRoomPage.CloseHearingButton));
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait));
            ExtensionMethods.FindElementEnabledWithWait(Driver, HearingRoomPage.IncomingFeedJudgeVideo, int.Parse(Config.DefaultElementWait));
            foreach (var participant in _hearing.Participant)
            {
                ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantWaitingRoomPage.ParticipantDetails($"{participant.Name.FirstName} {participant.Name.LastName}"), int.Parse(Config.DefaultElementWait)).Displayed.Should().BeTrue();
            }
        }

        [Then(@"the judge closes the hearing")]
        public void ThenTheJudgeClosesTheHearing()
        {
            Driver = GetDriver("Judge", _scenarioContext);
            _scenarioContext["driver"] = Driver;
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
            wait.Until(ExpectedConditions.ElementToBeClickable(HearingRoomPage.CloseHearingButton));
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait));
            Driver.FindElement(HearingRoomPage.CloseHearingButton).Click();
            Driver.FindElement(HearingRoomPage.ConfirmCloseHearingButton).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(JudgeWaitingRoomPage.EnterPrivateConsultationButton));
            Assert.IsTrue(Driver.FindElement(JudgeWaitingRoomPage.EnterPrivateConsultationButton).Displayed);
            // Assert participants are redirected back to the participants waiting room
            foreach (var participant in _hearing.Participant)
            { 
                if (!participant.Party.Name.ToLower().Contains("judge"))
                {
                    Driver = GetDriver($"#{participant.Party.Name}-{participant.Role.Name}", _scenarioContext);
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait));
                    var waitingRoomPage = Driver.PageSource;
                    var role = participant.Role.Name == "Solicitor" ? "Representative" : participant.Role.Name;
                    _scenarioContext["driver"] = Driver;
                    waitingRoomPage.Should().Contain(ParticipantWaitingRoomPage.ParticipantWaitingRoomClosedTitle);
                    Driver.FindElement(ParticipantWaitingRoomPage.ParticipantDetails($"{participant.Name.FirstName} {participant.Name.LastName}")).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.ParticipantDetails(participant.Party.Name)).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.ParticipantDetails(_hearing.Case.CaseNumber)).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.ChooseCameraAndMicButton).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.JoinPrivateMeetingButton).Displayed.Should().BeTrue();
                }
            }
        }
    }
}