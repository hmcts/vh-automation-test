using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumSpecFlow.Utilities;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using TestFramework;
using UISelenium.Pages;
using OpenQA.Selenium.Interactions;
using UI.Utilities;
using UI.Model;
using TestLibrary.Utilities;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;
using FluentAssertions;

namespace UI.Steps
{
    [Binding]
    public class CheckAlertsCommandCenterSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private Hearing _hearing;

        public readonly string CAMERA_ALERT = "Failed self-test (Camera)";
        public readonly string VIDEO_ALERT = "Failed self-test (Video)";
        public readonly string MICROPHONE_ALERT = "Failed self-test (Microphone)";
        public readonly string INCOMPLETE_ALERT = "Failed self-test(Incomplete Test)";
        public readonly string DISCONNECTED_ALERT = "Disconnected";

        CheckAlertsCommandCenterSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I login to VHO in video url as ""([^""]*)"" for existing hearing")]
        public void GivenILoginToVHOInVideoUrlAsForExistingHearing(string userName)
        {

            Driver = new DriverFactory().InitializeDriver(TestConfigHelper.browser);
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
                    Driver.FindElement(ParticipantHearingListPage.CameraWorkingNo)?.Click();
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();

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
                        TestFramework.ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }
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
                        TestFramework.ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }
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
                Driver = driver.Value;
                string participant = driver.Key.Split('#').FirstOrDefault();
                string cameraUrl = "";

                Driver = GetDriver(participant, _scenarioContext);
                _scenarioContext["driver"] = Driver;
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                ExtensionMethods.FindElementWithWait(Driver, ParticipantHearingListPage.SelectButton(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                //if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
                //{
                //    Driver.FindElement(ParticipantHearingListPage.ButtonNext).Click();
                //    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                //    Driver.FindElement(ParticipantHearingListPage.SwitchOnButton).Click();
                //    Driver.FindElement(ParticipantHearingListPage.WatchVideoButton).Click();
                //    if (SkipPracticeVideoHearingDemo)
                //    {
                //        cameraUrl = Driver.Url.Replace("practice-video-hearing", "hearing-rules");
                //        Driver.Navigate().GoToUrl(cameraUrl);
                //        Driver.SwitchTo().Alert().Accept();
                //    }
                //    else
                //    {
                //        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                //        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                //    }
                //    Driver.FindElement(ParticipantHearingListPage.NextButton).Click();
                //    Driver.FindElement(ParticipantHearingListPage.DeclareCheckbox).Click();
                //    Driver.FindElement(ParticipantHearingListPage.NextButton).Click();
                //}

                if (!(participant.ToLower().Contains("judge") || participant.ToLower().Contains("panel")))
                {
                    Driver.FindElement(ParticipantHearingListPage.ButtonNext).Click();
                    Driver.FindElement(ParticipantHearingListPage.ContinueButton).Click();
                    Driver.FindElement(ParticipantHearingListPage.SwitchOnButton).Click();
                    Driver.FindElement(ParticipantHearingListPage.WatchVideoButton).Click();
                    if (SkipPracticeVideoHearingDemo)
                    {
                        cameraUrl = Driver.Url.Replace("practice-video-hearing", "camera-working");
                        Driver.Navigate().GoToUrl(cameraUrl);
                        Driver.SwitchTo().Alert().Accept();
                    }
                    else
                    {
                        ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
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

        [Then(@"participant has joined and progressed to waiting room without completing self test disconnected")]
        public void ThenParticipantHasJoinedAndProgressedToWaitingRoomWithoutCompletingSelfTestDisconnected()
        {
            string cameraUrl = "";
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
                    if (SkipPracticeVideoHearingDemo)
                    {
                        ExtensionMethods.FindElementWithWait(Driver, GetReadyForTheHearingPage.NextButton, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait));
                        var oldUrl = Driver.Url;
                        cameraUrl = oldUrl.Replace("introduction", "participant/waiting-room");
                        Driver.Navigate().GoToUrl(cameraUrl);
                        Thread.Sleep(3000);
                        Driver.SwitchTo().Alert().Accept();
                    }
                    else
                    {
                        TestFramework.ExtensionMethods.FindElementEnabledWithWait(Driver, ParticipantHearingListPage.ContinueButton, 180).Click();
                        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultElementWait);
                    }
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantWaitingRoomPage.Returntovideohearinglist, _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait)).Click();
                }
            }

        }


        [When(@"the Video Hearings Officer check alerts for this hearing")]
        public void WhenTheVideoHearingsOfficerCheckAlertsForThisHearing()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.ViewHearings, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.SelectCaseNumber(_hearing.Case.CaseNumber), _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.HearingBtn, _scenarioContext).Click();

        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(No to Camera\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestNoToCameraParticipantFLName()
        {         
            _hearing = (Hearing)_scenarioContext["Hearing"];

            if (ExtensionMethods.IsElementExists(Driver, SelectYourHearingListPage.FailedAlert, _scenarioContext))
            {
                var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);

                for (int i = 1; i <= alerts.Count; i++)
                {
                    string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                    string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;

                    Assert.AreEqual(alertMsg, CAMERA_ALERT);
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
                }

            }

        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(No to Microphone\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestNoToMicrophoneParticipantFLName()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];

            if (ExtensionMethods.IsElementExists(Driver, SelectYourHearingListPage.FailedAlert, _scenarioContext))
            {
                var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);

                for (int i = 1; i <= alerts.Count; i++)
                {

                    string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                    string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;

                    Assert.AreEqual(alertMsg, MICROPHONE_ALERT);
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
                }

            }

        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(No to Video\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestNoToVideoParticipantFLName()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];

            if (ExtensionMethods.IsElementExists(Driver, SelectYourHearingListPage.FailedAlert, _scenarioContext))
            {
                var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);

                for (int i = 1; i <= alerts.Count; i++)
                {

                    string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                    string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;

                    Assert.AreEqual(alertMsg, VIDEO_ALERT);
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
                }

            }

        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(incomplete\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestIncompleteParticipantFLName()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];

            if (ExtensionMethods.IsElementExists(Driver, SelectYourHearingListPage.FailedAlert, _scenarioContext))
            {
                var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);

                for (int i = 1; i <= alerts.Count; i++)
                {

                    string alertMsg = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.AlertMsg(i.ToString()), _scenarioContext).Text;
                    string firstLastName = ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.FirstLastName(i.ToString()), _scenarioContext).Text;

                    Assert.AreEqual(alertMsg, INCOMPLETE_ALERT);
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.FirstName));
                    Assert.True(firstLastName.Contains(_hearing.Participant[i].Name.LastName));
                }

            }
        }

        [Then(@"the the Video Hearings Officer see the alert Failed self-test \(disconnected\) participant F & L name")]
        public void ThenTheTheVideoHearingsOfficerSeeTheAlertFailedSelf_TestDisconnectedParticipantFLName()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];

            if (ExtensionMethods.IsElementExists(Driver, SelectYourHearingListPage.FailedAlert, _scenarioContext))
            {
                var alerts = Driver.FindElements(SelectYourHearingListPage.FailedAlert);

                for (int i = 0; i <= alerts.Count; i++)
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
}