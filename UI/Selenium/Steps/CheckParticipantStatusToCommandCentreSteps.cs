using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using TestFramework;
using OpenQA.Selenium.Interactions;
using UI.Utilities;
using UI.Model;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using FluentAssertions;
using System.Collections.Generic;
using UI.Pages;

namespace UI.Steps
{
    [Binding]
    public class CheckParticipantStatusToCommandCentreSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        HearingListSteps hearingListSteps;

        CheckParticipantStatusToCommandCentreSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"participants have joined the hearing waiting room")]
        public void ThenParticipantsHaveJoinedTheHearingWaitingRoom()
        {
             int participantNum = 0;
            _hearing = (Hearing)_scenarioContext["Hearing"];
            hearingListSteps = new HearingListSteps(_scenarioContext);
            foreach (var participant in _hearing.Participant)
            {   
                Driver = GetDriver(participant.Id, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                if (!(participant.Party.Name.ToLower().Contains("judge") || participant.Party.Name.ToLower().Contains("panel")))
                {
                    if (participantNum == 1)
                    {
                        hearingListSteps.ProceedToWaitingRoom(participant.Id, skipToWaitingRoom: true);
                    }                    
                }
                participantNum++;
            }
        }

        [Then(@"participants have joined the hearing waiting room without Judge")]
        public void ThenParticipantsHaveJoinedTheHearingWaitingRoomWithoutJudge()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            hearingListSteps = new HearingListSteps(_scenarioContext);
            foreach (var participant in _hearing.Participant)
            {
                Driver = GetDriver(participant.Id, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                if (!(participant.Party.Name.ToLower().Contains("judge")))
                {
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                    if (!participant.Party.Name.ToLower().Contains("panel"))
                    {
                        hearingListSteps.ProceedToWaitingRoom(participant.Id, skipToWaitingRoom: true);
                    }
                }
            }
        }

        [Then(@"the Video Hearings Officer should able to see the status")]
        public void ThenTheVideoHearingsOfficerShouldAbleToSeeTheStatus()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                Driver = GetDriver(participant, _scenarioContext);
                if (participant.Equals("VHO"))
                {
                    ExtensionMethods.WaitForElementVisible(Driver, VHOHearingListPage.ParticipantName);
                    Driver.FindElement(VHOHearingListPage.ParticipantStatusInHearing).Displayed.Should().BeTrue();
                    Driver.FindElement(VHOHearingListPage.ParticipantStatusJoining).Displayed.Should().BeTrue();
                    Driver.FindElement(VHOHearingListPage.ParticipantStatusNotSignedIn).Displayed.Should().BeTrue();
                }               
            }
        }

        [Then(@"the Video Hearings Officer should able to view the status")]
        public void ThenTheVideoHearingsOfficerShouldAbleToViewTheStatus()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                Driver = GetDriver(participant, _scenarioContext);
                if (participant.Equals("VHO"))
                {
                    ExtensionMethods.WaitForElementVisible(Driver, VHOHearingListPage.ParticipantName, 120);
                    Driver.FindElement(VHOHearingListPage.ParticipantStatusAvailable).Displayed.Should().BeTrue();
                    Driver.FindElement(VHOHearingListPage.ParticipantStatusUnavailable).Displayed.Should().BeTrue();
                    Driver.FindElement(VHOHearingListPage.ParticipantStatusInConsultation).Displayed.Should().BeTrue();                    
                }
            }
        }
    }
}