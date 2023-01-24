using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using System.Collections.Generic;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for checking alerts in Command Center
    ///</summary>
    public class CheckAlertsCommandCenterSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public readonly string CAMERA_ALERT = "Failed self-test (Camera)";
        public readonly string VIDEO_ALERT = "Failed self-test (Video)";
        public readonly string MICROPHONE_ALERT = "Failed self-test (Microphone)";
        public readonly string INCOMPLETE_ALERT = "Failed self-test (Incomplete Test)";
        public readonly string DISCONNECTED_ALERT = "Disconnected";
        CheckAlertsCommandCenterSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I login to VHO in video url as ""([^""]*)"" for existing hearing")]
        public void GivenILoginToVHOInVideoUrlAsForExistingHearing(string userName)
        {

            Driver = new DriverFactory().InitializeDriver(Config.BrowserType);
            _scenarioContext["driver"] = Driver;
            Driver.Navigate().GoToUrl(Config.VideoUrl);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
            wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.UsernameTextfield));
            _scenarioContext.UpdatePageName("Video Web Login");
            Login(userName, Config.UserPassword);
        }

        public void Login(string username, string password)
        {
            ExtensionMethods.FindElementWithWait(Driver, LoginPage.UsernameTextfield, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).SendKeys(username);
            Driver.FindElement(LoginPage.Next).Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
            wait.Until(ExpectedConditions.ElementIsVisible(LoginPage.PasswordField));
            wait.Until(ExpectedConditions.ElementToBeClickable(LoginPage.SignIn));
            wait.Until(ExpectedConditions.ElementToBeClickable(LoginPage.BackButton));
            Driver.FindElement(LoginPage.PasswordField).SendKeys(password);
            ExtensionMethods.FindElementWithWait(Driver, LoginPage.SignIn, _scenarioContext).Click();
        }

        [Then(@"participant has joined and progressed to waiting room without completing self test")]
        public void ThenParticipantHasJoinedAndProgressedToWaitingRoomWithoutCompletingSelfTest()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                Driver = GetDriver(participant, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
                {
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.ButtonNext, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.ContinueButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SwitchOnButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.WatchVideoButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                    // Assert video is playing
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.ContinueButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
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
                    _hearing.HearingId = Driver.Url.Split('/').LastOrDefault();
                    _scenarioContext["Hearing"] = _hearing;
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.CameraWorkingNo, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.ContinueButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                }
            }
        }

        [Then(@"participant has joined and progressed to waiting room without completing self test microphone")]
        public void ThenParticipantHasJoinedAndProgressedToWaitingRoomWithoutCompletingSelfTestMicrophone()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();

                Driver = GetDriver(participant, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
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
                        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }
                    _hearing.HearingId = Driver.Url.Split('/').LastOrDefault();
                    _scenarioContext["Hearing"] = _hearing;
                    Driver.FindElement(ParticipantHearingListPage.CameraWorkingYes)?.Click();
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                    Driver.FindElement(ParticipantHearingListPage.MicrophoneWorkingNo).Click();
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                }
            }
        }

        [Then(@"participant has joined and progressed to waiting room without completing self test video")]
        public void ThenParticipantHasJoinedAndProgressedToWaitingRoomWithoutCompletingSelfTestVideo()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                Driver = GetDriver(participant, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
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
                        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }
                    _hearing.HearingId = Driver.Url.Split('/').LastOrDefault();
                    _scenarioContext["Hearing"] = _hearing;
                    Driver.FindElement(ParticipantHearingListPage.CameraWorkingYes)?.Click();
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                    Driver.FindElement(ParticipantHearingListPage.MicrophoneWorkingYes).Click();
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                    Driver.FindElement(ParticipantHearingListPage.VideoWorkingNo).Click();
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                }
            }
        }

        [Then(@"participant has joined and progressed to waiting room without completing self test Incomplete")]
        public void ThenParticipantHasJoinedAndProgressedToWaitingRoomWithoutCompletingSelfTestIncomplete()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                var cameraUrl = "";
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                Driver = GetDriver(participant, _scenarioContext);
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
                {
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.ButtonNext, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.ContinueButton, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SwitchOnButton, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.WatchVideoButton, _scenarioContext).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.OutgoingStreamVideo);
                    ExtensionMethods.WaitForElementNotVisible(Driver, ParticipantHearingListPage.OutgoingStreamVideo, 90);
                    ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, null).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.ContinueButton);
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.CameraWorkingYes, _scenarioContext, TimeSpan.FromSeconds(4));
                    if (!Driver.Url.Contains("camera-working"))
                    {
                        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, null).Click();
                        ExtensionMethods.WaitForElementVisible(Driver, ParticipantHearingListPage.CameraWorkingYes);
                    }
                    _hearing.HearingId = Driver.Url.Split('/').LastOrDefault();
                    _scenarioContext["Hearing"] = _hearing;
                    if (SkipPracticeVideoHearingDemo)
                    {
                        cameraUrl = Driver.Url.Replace("camera-working", "participant/waiting-room");
                        Driver.Navigate().GoToUrl(cameraUrl);
                        Driver.SwitchTo().Alert().Accept();
                    }
                }
            }
        }

        [Then(@"participant has joined and progressed to waiting room without completing self test disconnected")]
        public void ThenParticipantHasJoinedAndProgressedToWaitingRoomWithoutCompletingSelfTestDisconnected()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            var cameraUrl = "";
            foreach (var driver in (Dictionary<string, IWebDriver>)_scenarioContext["drivers"])
            {
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                Driver = GetDriver(participant, _scenarioContext);
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
                {
                    if (SkipPracticeVideoHearingDemo)
                    {
                        ExtensionMethods.FindElementWithWait(Driver, GetReadyForTheHearingPage.NextButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait));
                        var oldUrl = Driver.Url;
                        cameraUrl = oldUrl.Replace("introduction", "participant/waiting-room");
                        _hearing.HearingId = Driver.Url.Split('/').LastOrDefault();
                        _scenarioContext["Hearing"] = _hearing;
                        Driver.Navigate().GoToUrl(cameraUrl);
                        Driver.SwitchTo().Alert().Accept();
                    }
                    else
                    {
                        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }

                    ExtensionMethods.WaitForElementVisible(Driver, ParticipantWaitingRoomPage.ChooseCameraAndMicButton);
                    Thread.Sleep(3000); // Needed here as without, tests fail
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantWaitingRoomPage.Returntovideohearinglist, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                }
            }
        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(No to Camera\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestNoToCameraParticipantFLName()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _hearing = (Hearing)_scenarioContext["Hearing"];
            var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);
            alerts.Count.Should().BeGreaterThan(0);
            for (int i = 1; i <= alerts.Count; i++)
            {
                string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;
                Assert.AreEqual(alertMsg, CAMERA_ALERT);
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
            }
        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(No to Microphone\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestNoToMicrophoneParticipantFLName()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _hearing = (Hearing)_scenarioContext["Hearing"];
            var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);
            alerts.Count.Should().BeGreaterThan(0);
            for (int i = 1; i <= alerts.Count; i++)
            {
                string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;
                Assert.AreEqual(alertMsg, MICROPHONE_ALERT);
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
            }
        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(No to Video\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestNoToVideoParticipantFLName()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _hearing = (Hearing)_scenarioContext["Hearing"];
            var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);
            alerts.Count.Should().BeGreaterThan(0);
            for (int i = 1; i <= alerts.Count; i++)
            {
                string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;
                Assert.AreEqual(alertMsg, VIDEO_ALERT);
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
            }
        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(incomplete\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestIncompleteParticipantFLName()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _hearing = (Hearing)_scenarioContext["Hearing"];
            var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);
            alerts.Count.Should().BeGreaterThan(0);
            string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg("1"), _scenarioContext).Text;
            string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName("1"), _scenarioContext).Text;
            Assert.AreEqual(alertMsg, INCOMPLETE_ALERT);
            Assert.True(firstLastName.Contains(_hearing.Participant[1].Name.FirstName));
            Assert.True(firstLastName.Contains(_hearing.Participant[1].Name.LastName));
        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(disconnected\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestDisconnectedParticipantFLName()
        {
            Driver = GetDriver(_hearing.Participant.Where(a => a.Role.Name.ToLower() == "vho").FirstOrDefault().Id, _scenarioContext);
            _hearing = (Hearing)_scenarioContext["Hearing"];
            var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);
            alerts.Count.Should().BeGreaterThan(0);
            for (int i = 1; i <= alerts.Count; i++)
            {
                string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;
                Assert.AreEqual(alertMsg, DISCONNECTED_ALERT);
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
            }
        }
    }
}