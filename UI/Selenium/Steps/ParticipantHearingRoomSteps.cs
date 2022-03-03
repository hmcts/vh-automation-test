using SeleniumSpecFlow.Utilities;
using System;
using TechTalk.SpecFlow;
using UISelenium.Pages;
using TestFramework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Linq;
using UI.Model;

namespace UI.Steps
{
    [Binding]
    public class ParticipantHearingRoomSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public ParticipantHearingRoomSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
        }

        [When(@"participants? switch off the video")]
        public void ThenParticipantSwitchOffTheVideo()
        {
            foreach (var participant in _hearing.Participant)
            {
                if(participant.VideoOff)
                {
                    Driver = GetDriver(participant.Id, _scenarioContext);
                    var elementVideoOff = ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingRoomPage.ToggleVideoButton, _scenarioContext);
                    elementVideoOff.Click();
                }
            }
        }
    }
}
