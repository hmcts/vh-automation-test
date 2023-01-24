using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using FluentAssertions;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Hearing List page
    ///</summary>
    public class HearingListSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private Hearing _hearing;
        HearingRoomSteps hearingRoomSteps;
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

        [Then(@"all participants have joined the hearing waiting room with SkipToWaitingRoom")]
        public void ThenAllParticipantsHaveJoinedTheHearingWaitingRoomWithSkipToWaitingRoom()
        {
            SignAllParticipantsIn(skipPreSetUpSteps: true);
        }

        public void SignAllParticipantsIn(bool skipPreSetUpSteps = false)
        {
            foreach(var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"]) 
            {
                Driver = driver.Value;
                ProceedToWaitingRoom(driver.Key.Split('#').FirstOrDefault(), skipPreSetUpSteps);
            }
        }

        public void ProceedToWaitingRoom(string participant, bool skipToWaitingRoom = false)
        {
            Driver = GetDriver(participant, _scenarioContext);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
            ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber));
            Driver.FindElement(ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber)).Click();
            if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
            {
                ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.ButtonNext);
                _hearing.HearingId = Driver.Url.Split('/').LastOrDefault();
                _scenarioContext["Hearing"] = _hearing;
                if (skipToWaitingRoom)
                {
                    string cameraUrl = Driver.Url.Replace("introduction", "participant/waiting-room");
                    Driver.Navigate().GoToUrl(cameraUrl);
                    Driver.SwitchTo().Alert().Accept();
                }
                else
                {
                    Driver.FindElement(ParticipantHearingListPage.ButtonNext).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.ContinueButton);
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.SwitchOnButton);
                    Driver.RetryClick(ParticipantHearingListPage.SwitchOnButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait));
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.WatchVideoButton);
                    Driver.FindElement(ParticipantHearingListPage.WatchVideoButton).Click();
                    // Assert video is playing
                    Driver.RetryClick(ParticipantHearingListPage.ContinueButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait));
                    if (SkipPracticeVideoHearingDemo)
                    {
                        string cameraUrl = Driver.Url.Replace("practice-video-hearing", "camera-working");
                        Driver.Navigate().GoToUrl(cameraUrl);
                        Driver.SwitchTo().Alert().Accept();
                    }
                    else
                    {
                        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    Driver.FindElement(ParticipantHearingListPage.CameraWorkingYes)?.Click();
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

        [When(@"selects current hearing")]
        public void WhenSelectsCurrentHearing()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _scenarioContext["driver"] = Driver;
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.CaseNameListItem(_hearing.HearingId), 120);
            ExtensionMethods.MoveToElement(Driver, HearingListPage.CaseNameListItem(_hearing.HearingId), _scenarioContext);
            ExtensionMethods.FindElementWithWait(Driver, HearingListPage.CaseNameListItem(_hearing.HearingId), _scenarioContext).Click();
        }

        [When(@"starts a consultation with a judge")]
        public void WhenStartsAConsultationWithAJudge()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _scenarioContext["driver"] = Driver;
            ExtensionMethods.WaitForElementVisible(Driver, ConsultationRoomPage.WaitingRoomIframe);
            ExtensionMethods.SwitchToIframe(Driver, ConsultationRoomPage.WaitingRoomIframe);
            ExtensionMethods.WaitForElementVisible(Driver, ConsultationRoomPage.WaitingRoomJudgeLink);
            Driver.FindElement(ConsultationRoomPage.WaitingRoomJudgeLink).Click();
            System.Threading.Thread.Sleep(500); // THIS IS NEEDED HERE, NONE OF THE WAITS WORK 
            var d = Driver.FindElements(ConsultationRoomPage.PrivateConsultation).Where(a => a.Text.Contains("Private consultation")).FirstOrDefault();
            d.Click();
            ExtensionMethods.WaitForElementVisible(Driver, ConsultationRoomPage.HearingListConsultationRooms);
            Assert.That(Driver.WindowHandles.Count, Is.EqualTo(2));
            SwitchToWindowByTitle("Private Consultation");
            ExtensionMethods.WaitForElementVisible(Driver, ConsultationRoomPage.SelfViewButton);
        }

        [Then(@"check judge is in the consultation room")]
        public void ThenCheckJudgeIsInTheConsultationRoom()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id, _scenarioContext);
            _scenarioContext["driver"] = Driver;
            ExtensionMethods.WaitForElementVisible(Driver, ConsultationRoomPage.LeaveButtonDesktop);
            ExtensionMethods.IsElementVisible(Driver, ConsultationRoomPage.LeaveButtonDesktop, null).Should().BeTrue(); 
            Assert.True(Driver.Url.Contains("/judge/waiting-room"));
        }

        [Then(@"closes the consultation")]
        public void ThenClosesTheConsultation()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _scenarioContext["driver"] = Driver;
            SwitchToWindowByTitle("Private Consultation");
            ExtensionMethods.FindElementEnabledWithWait(Driver, ConsultationRoomPage.CloseButton).Click();
            SwitchToWindowByTitle("Video Hearings - VHO Admin dashboard");
            Assert.That(Driver.WindowHandles.Count, Is.EqualTo(1));
        }

        [Then(@"check the judge returns to the waiting room")]
        public void ThenCheckTheJudgeReturnsToTheWaitingRoom()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id, _scenarioContext);
            _scenarioContext["driver"] = Driver;
            Driver.FindElement(ParticipantWaitingRoomPage.ChooseCameraAndMicButton).Displayed.Should().BeTrue();
            Driver.FindElement(ParticipantWaitingRoomPage.StartVideoHearingButton).Displayed.Should().BeTrue();
        }

        [When(@"selects the messages tab")]
        public void WhenSelectsTheMessagesTab()
        {
            hearingRoomSteps = new HearingRoomSteps(_scenarioContext);
            ExtensionMethods.WaitForElementVisible(Driver, ConsultationRoomPage.WaitingRoomIframe);
            Driver.FindElement(HearingListPage.MessagesTabButton).Click();
        }

        [When(@"is able to see and send messages to the hearing participants")]
        public void WhenIsAbleToSeeAndSendMessagesToTheHearingParticipants()
        {
            var msg = "Hi Judge, this is a test message";
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id, _scenarioContext);
            hearingRoomSteps.WhenTheJudgeCanSendAMessageToAVHOUsingViaHearingRoomChatPanel();
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            Driver.FindElements(HearingListPage.IMAvailableParticipant)?.FirstOrDefault()?.Click();
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.InstantMessageInput);
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.ChatWindow);
            Driver.FindElement(HearingListPage.ChatWindow).Text.Should().Contain("Hi, could you please join the hearing");
            Driver.FindElement(HearingListPage.InstantMessageInput).SendKeys(msg);
            Driver.FindElement(HearingListPage.InstantMessageButton).Click();
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.DivContainsText(msg));
            Driver.FindElement(HearingListPage.ChatWindow).Text.Should().Contain(msg);
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.FindElementWithWait(Driver, HearingRoomPage.JudgeMessageSent(msg), _scenarioContext).Displayed.Should().BeTrue();
        }

        [Then(@"vho can IM both judge and participants while in the waiting room")]
        public void ThenVhoCanIMBothJudgeAndParticipants()
        {
            var judgeMsg = "Hi Judge, I can see you are in the waiting room now";
            var participantMsg = "Hello, welcome to the waiting room, enjoy your break";
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            Driver.FindElements(HearingListPage.IMAvailableParticipant)?.FirstOrDefault()?.Click();
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.InstantMessageInput);
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.ChatWindow);
            Driver.FindElement(HearingListPage.InstantMessageInput).SendKeys(judgeMsg);
            Driver.FindElement(HearingListPage.InstantMessageButton).Click();
            ExtensionMethods.WaitForTextPresent(Driver, judgeMsg);
            Driver.FindElements(HearingListPage.IMAvailableParticipant)?.LastOrDefault()?.Click();
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.InstantMessageInput);
            Driver.FindElement(HearingListPage.InstantMessageInput).SendKeys(participantMsg);
            Driver.FindElement(HearingListPage.InstantMessageButton).Click();
            ExtensionMethods.WaitForTextPresent(Driver, participantMsg);
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.WaitForTextPresent(Driver, judgeMsg);
            judgeMsg = $"Send me a message to my email at {_hearing.Participant.Where(a => a.Role.Name.ToLower() == "judge").FirstOrDefault().Id}";
            Driver.FindElement(HearingListPage.InstantMessageInput).SendKeys(judgeMsg);
            Driver.FindElement(HearingListPage.InstantMessageButton).Click();
            ExtensionMethods.WaitForTextPresent(Driver, judgeMsg);
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() != "judge" && a.Role.Name.ToLower() != "vho").FirstOrDefault().Id, _scenarioContext);
            ExtensionMethods.WaitForTextPresent(Driver, participantMsg);
            participantMsg = "Thank you, see you later #Monday";
            Driver.FindElement(HearingListPage.InstantMessageInput).SendKeys(participantMsg);
            Driver.FindElement(HearingListPage.InstantMessageButton).Click();
            ExtensionMethods.WaitForTextPresent(Driver, participantMsg);
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            Driver.FindElements(HearingListPage.IMAvailableParticipant)?.FirstOrDefault()?.Click();
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.InstantMessageInput);
            ExtensionMethods.WaitForTextPresent(Driver, judgeMsg);
            Driver.FindElements(HearingListPage.IMAvailableParticipant)?.LastOrDefault()?.Click();
            ExtensionMethods.WaitForElementVisible(Driver, HearingListPage.InstantMessageInput);
            ExtensionMethods.WaitForTextPresent(Driver, participantMsg);
        }
    }
}