using System;
using TechTalk.SpecFlow;
using UI.Model;
using FluentAssertions;
using TestFramework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Linq;
using UI.Pages;
using UI.Steps.CommonActions;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Waiting room page
    ///</summary>
    public class WaitingRoomPageSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private Hearing _hearing;
        HearingListSteps hearingListSteps;
        HearingRoomSteps hearingRoomSteps;
        LogoffSteps logoffSteps;

        public WaitingRoomPageSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
            hearingListSteps = new HearingListSteps(scenarioContext);
            hearingRoomSteps = new HearingRoomSteps(scenarioContext);
            logoffSteps = new LogoffSteps(scenarioContext);
        }

        [Then(@"the judge starts the hearing")]
        public void ThenTheJudgeStartsTheHearing()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower().Contains("judge")).FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.WaitForElementVisible(Driver, ParticipantWaitingRoomPage.StartVideoHearingButton);
            ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantWaitingRoomPage.StartVideoHearingButton).Click();
            ExtensionMethods.WaitForElementVisible(Driver, ParticipantWaitingRoomPage.ConfirmStartButton);
            ExtensionMethods.FindElementWithWait(Driver, ParticipantWaitingRoomPage.ConfirmStartButton, _scenarioContext).Click();
            _scenarioContext.UpdatePageName("Judge Waiting Room");
        }

        [When(@"the judge selects Enter consultation room")]
        public void WhenJudgeSelectsEnterConsultation()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower().Contains("judge")).FirstOrDefault().Id, _scenarioContext);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
            wait.Until(ExpectedConditions.ElementToBeClickable(JudgeWaitingRoomPage.EnterPrivateConsultationButton));
            ExtensionMethods.RetryClick(Driver, JudgeWaitingRoomPage.EnterPrivateConsultationButton, _scenarioContext);
        }

        [When(@"the panel member selects Enter consultation room")]
        public void WhenJoHSelectsEnterConsultation()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower().Contains("panel")).FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.WaitForElementVisible(Driver, JudgeWaitingRoomPage.EnterPrivateConsultationButton, 20);
            ExtensionMethods.RetryClick(Driver, JudgeWaitingRoomPage.EnterPrivateConsultationButton, _scenarioContext);
        }

        [Then(@"all participants are redirected to the hearing room when the judge resumes the video hearing")]
        public void ThenAllParticipantsAreRedirectedToTheHearingRoomWhenTheJudgeResumesTheVideoHearing()
        {
            ResumeVideoHearing();
            new HearingRoomSteps(_scenarioContext).ThenTheJudgeChecksThatAllParticipantsHaveJoinedTheHearingRoom();
        }

        public void ResumeVideoHearing()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower().Contains("judge")).FirstOrDefault().Id, _scenarioContext);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
            wait.Until(ExpectedConditions.ElementToBeClickable(JudgeWaitingRoomPage.ResumeVideoHearing));
            ExtensionMethods.FindElementWithWait(Driver, JudgeWaitingRoomPage.ResumeVideoHearing, _scenarioContext).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(ParticipantWaitingRoomPage.ConfirmStartButton));
            ExtensionMethods.FindElementWithWait(Driver, ParticipantWaitingRoomPage.ConfirmStartButton, _scenarioContext).Click();
        }
         
        public void CheckParticipantsAreInWaitingRoom()
        {
            foreach (var participant in _hearing.Participant)
            {
                if (!(participant.Role.Name.ToLower().Contains("judge") || participant.Party.Name.ToLower().Contains("panel")) && !participant.Party.Name.ToLower().Contains("vho"))
                {
                    Driver = GetDriver($"#{participant.Party.Name}-{participant.Role.Name}", _scenarioContext);
                    WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
                    wait.Until(ExpectedConditions.ElementToBeClickable(ParticipantWaitingRoomPage.StartPrivateMeetingButton));
                    wait.Until(ExpectedConditions.ElementToBeClickable(ParticipantWaitingRoomPage.ChooseCameraAndMicButton));
                    var waitingRoomPage = Driver.PageSource;
                    bool isPaused = waitingRoomPage.Contains("Your video hearing is paused") ? true : false;
                    var role = participant.Role.Name == "Solicitor" ? "Representative" : participant.Role.Name;
                    if(isPaused)
                    {
                        waitingRoomPage.Should().Contain(ParticipantWaitingRoomPage.ParticipantWaitingRoomPausedTitle);
                    }
                    else
                    {
                        waitingRoomPage.Should().Contain(ParticipantWaitingRoomPage.ParticipantWaitingRoomClosedTitle);
                    }
                    Driver.FindElement(ParticipantWaitingRoomPage.ParticipantDetails($"{participant.Name.FirstName} {participant.Name.LastName}")).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.ParticipantDetails(participant.Party.Name)).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.ParticipantDetails(_hearing.Case.CaseNumber)).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.ChooseCameraAndMicButton).Displayed.Should().BeTrue();
                    Driver.FindElement(ParticipantWaitingRoomPage.StartPrivateMeetingButton).Displayed.Should().BeTrue();
                }
            }
        }

        [Then(@"the judge signs into the hearing")]
        public void ThenTheJudgeSignsIntoTheHearing()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower().Contains("judge")).FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext).Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
            wait.Until(ExpectedConditions.ElementToBeClickable(ParticipantWaitingRoomPage.StartVideoHearingButton));
        }

        [Then(@"assert the judge sees the correct status for each participant as NOT SIGNED IN")]
        public void ThenAssertTheJudgeSeesTheCorrectStatusForEachParticipantAsNOTSIGNEDIN()
        {
            var judge = _hearing.Participant.Where(a => a.Id.ToLower().Contains("judge")).FirstOrDefault();
            Driver = GetDriver(judge.Role.Name, _scenarioContext);
            var noOfParticipantsNotSignedIn = Driver.FindElements(ParticipantWaitingRoomPage.NotSignedInStatus)?.Count;
            var numberOParticipantsInAHearing = _hearing.Participant.Count - 1;
            noOfParticipantsNotSignedIn.Value.Should().Be(numberOParticipantsInAHearing);
        }

        [Then(@"assert the judge and participant's status change as each participant join the waiting room")]
        public void ThenAssertTheJudgeAndParticipantsStatusChangeAsMoreParticipantsJoinTheWaitingRoom()
        {
            var noOfParticipantsNotSignedIn = Driver.FindElements(ParticipantWaitingRoomPage.NotSignedInStatus)?.Count;
            var numberOParticipantsInAHearing = _hearing.Participant.Count - 1;
            foreach (var participant in _hearing.Participant)
            {
                if (!participant.Role.Name.ToLower().Contains("judge"))
                {
                    Driver = GetDriver(participant.Id, _scenarioContext);
                    hearingListSteps.ProceedToWaitingRoom(participant.Id, skipToWaitingRoom: true);
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantWaitingRoomPage.StartPrivateMeetingButton);
                    ExtensionMethods.IsElementVisible(Driver, ParticipantWaitingRoomPage.StartPrivateMeetingButton, null).Should().BeTrue();
                    var unAvailableParticipants = noOfParticipantsNotSignedIn.Value == 1 ? 0 : Driver.FindElements(ParticipantWaitingRoomPage.UnAvailableStatus)?.Count;
                    (noOfParticipantsNotSignedIn.Value - 1).Should().Be(unAvailableParticipants);
                    // Assert Judge status changed
                    var judge = _hearing.Participant.Where(a => a.Id.ToLower().Contains("judge")).FirstOrDefault();
                    Driver = GetDriver(judge.Role.Name, _scenarioContext);
                    System.Threading.Thread.Sleep(3000); // Judges status takes a couple of sec to update
                    var newNotSignedIn = noOfParticipantsNotSignedIn.Value == 1 ? 0 : Driver.FindElements(ParticipantWaitingRoomPage.NotSignedInStatus)?.Count;
                    newNotSignedIn.Should().BeLessThan(noOfParticipantsNotSignedIn.Value);
                    noOfParticipantsNotSignedIn = noOfParticipantsNotSignedIn.Value == 1 ? 0 : Driver.FindElements(ParticipantWaitingRoomPage.NotSignedInStatus)?.Count; 
                }
            }
        }

        [Then(@"the judge starts the hearing and checks that all participants have joined the hearing room")]
        public void ThenTheJudgeStartsTheHearingAndChecksThatAllParticipantsHaveJoinedTheHearingRoom()
        {
            ThenTheJudgeStartsTheHearing();
            hearingRoomSteps.ThenTheJudgeChecksThatAllParticipantsHaveJoinedTheHearingRoom();
        }

        [When(@"each have their status as CONNECTED and Available on judge's and participants screen respectively")]
        public void WhenEachHaveTheirStatusAsCONNECTEDAndAvailableOnJudgeScreenRespectively()
        {
            var judge = _hearing.Participant.Where(a => a.Id.ToLower().Contains("judge")).FirstOrDefault();
            Driver = GetDriver(judge.Role.Name, _scenarioContext);
            var numberOParticipantsInAHearing = _hearing.Participant.Count - 1;
            var noOfParticipantsConnected = Driver.FindElements(ParticipantWaitingRoomPage.ConnectedStatus)?.Count;
            noOfParticipantsConnected.Value.Should().Be(numberOParticipantsInAHearing);
        }

        [When(@"status should show Disconnected and Unavailable for judge and participants respectively when a participant logs off or close browser")]
        public void WhenStatusShouldShowDisconnectedAndUnavailableForJudgeAndParticipantsRespectivelyWhenAParticipantLogsOffOrCloseBrowser()
        {
            var participants = _hearing.Participant.Where(a => !a.Id.ToLower().Contains("judge"));
            var judge = _hearing.Participant.Where(a => a.Id.ToLower().Contains("judge")).FirstOrDefault();
            var disconnectedParticipant = participants.FirstOrDefault();
            Driver = GetDriver($"{disconnectedParticipant.Id}#{disconnectedParticipant.Party.Name}-{disconnectedParticipant.Role.Name}", _scenarioContext);
            logoffSteps.ThenILogOff();
            // check for DISCONECTED status on judges waiting room screen
            Driver = GetDriver(judge.Role.Name, _scenarioContext);
            ExtensionMethods.WaitForElementVisible(Driver, ParticipantWaitingRoomPage.DisconnectedStatus);
            Driver.FindElements(ParticipantWaitingRoomPage.DisconnectedStatus)?.Count.Should().BeGreaterThan(0);
            //check for unavailable status on other participants waiting room screen
            foreach(var participant in participants)
            {
                if(participant != disconnectedParticipant)
                {
                    Driver = GetDriver(participant.Id, _scenarioContext);
                    ExtensionMethods.IsElementVisible(Driver, ParticipantWaitingRoomPage.UnAvailableStatus, null).Should().BeTrue();
                }
            }
        }

        [When(@"the invite into consultation room gets accepted")]
        public void WhenTheJudgeAcceptsTheIncomingInvite()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.FindElementWithWait(Driver, JudgeWaitingRoomPage.ToastInviteAcceptButton, _scenarioContext).Click();
            ExtensionMethods.WaitForElementNotVisible(Driver, JudgeWaitingRoomPage.ToastInviteAcceptButton);
        }

        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
        }
    }
}
