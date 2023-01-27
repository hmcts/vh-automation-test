using TechTalk.SpecFlow;
using Selenium.Axe;
using FluentAssertions;
using TestFramework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using UI.Utilities;
using OpenQA.Selenium;
using System.Linq;
using UI.Pages;
using UI.Pages.PageElements;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for checking accessibility for Video Booking
    ///</summary>
    public class AccessibilitySteps_VideoBooking : ObjectFactory
    {
        private ScenarioContext _scenarioContext;
        public string username = "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net";
        private LoginPageSteps loginSteps;
        private DashboardSteps dashboardSteps;
        private HearingScheduleSteps hearingScheduleSteps;
        private CreateHearingDetails createHearingDetails;
        ParticipantsSteps participantsSteps;
        HearingAssignJudgeSteps hearingAssignJudgeSteps;
        VideoAccessSteps videoAccessSteps;
        OtherInformationSteps otherInformationSteps;
        SummaryPageSteps summaryPageSteps;
        private AxeBuilder axeResult;
        public string PageName;

        public AccessibilitySteps_VideoBooking(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            loginSteps = new LoginPageSteps(scenarioContext);
            dashboardSteps = new DashboardSteps(scenarioContext);
            createHearingDetails = new CreateHearingDetails(scenarioContext);
            participantsSteps = new ParticipantsSteps(scenarioContext);
            videoAccessSteps = new VideoAccessSteps(scenarioContext);
            otherInformationSteps = new OtherInformationSteps(scenarioContext);
            summaryPageSteps = new SummaryPageSteps(scenarioContext);
        }

        [Given(@"I'm on the ""([^""]*)"" page")]
        public void GivenImOnThePage(string pageName)
        {
            PageName = pageName;
            loginSteps.GivenILogInAs(username);
        }

        [Then(@"the page should be accessible")]
        public void ThenThePageShouldBeAccessible()
        {
            CheckAccessibilityCompliance(PageName);
        }

        private void CheckAccessibilityCompliance(string pageName)
        {
            ExtensionMethods.FindElementWithWait(Driver, DashboardPage.BookHearingButton, _scenarioContext);
            switch(pageName)
            {
                case "Hearing Details":
                    dashboardSteps.GivenISelectBookAHearing();
                    AxeAnalyze(pageName);
                    break;
                case "Hearing Schedule":
                    ProceedToPage("Hearing Details");
                    var table = StepsHelper.Set.HearingDetailsData();
                    createHearingDetails.GivenIWantToCreateAHearingWithCaseDetails(table);
                    ExtensionMethods.FindElementWithWait(Driver, HearingSchedulePage.HearingDate, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "AssignJudge":
                    ProceedToPage("Hearing Schedule");
                    hearingScheduleSteps = new HearingScheduleSteps(_scenarioContext);
                    table = StepsHelper.Set.HearingScheduleData();
                    hearingScheduleSteps.GivenTheHearingHasTheFollowingScheduleDetails(table);
                    ExtensionMethods.FindElementWithWait(Driver, HearingAssignJudgePage.JudgeEmail, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Participants":
                    ProceedToPage("AssignJudge");
                    hearingAssignJudgeSteps = new HearingAssignJudgeSteps(_scenarioContext);
                    table = StepsHelper.Set.JudgeData();
                    hearingAssignJudgeSteps.GivenIWantToAssignAJudgeWithCourtroomDetails(table);
                    new SelectElement(ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.PartyDropdown, _scenarioContext));
                    AxeAnalyze(pageName);
                    break;
                case "Video Access Points":
                    ProceedToPage("Participants");
                    table = StepsHelper.Set.ParticipantsData();
                    participantsSteps.GivenIWantToCreateAHearingFor(table);
                    ExtensionMethods.GetSelectElementWithText(Driver, VideoAccessPointsPage.DefenceAdvocate(0), "None", _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Other Information":
                    ProceedToPage("Video Access Points");
                    table = StepsHelper.Set.VideoAccessPointData();
                    videoAccessSteps.GivenWithVideoAccessPointsDetails(table);
                    ExtensionMethods.FindElementWithWait(Driver, OtherInformationPage.OtherInfo, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Summary":
                    ProceedToPage("Other Information");
                    table = StepsHelper.Set.SetOtherInfoData();
                    otherInformationSteps.GivenISetAnyOtherInformation(table);
                    ExtensionMethods.FindElementWithWait(Driver, SummaryPage.BookButton, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Booking Confirmation":
                    ProceedToPage("Summary");
                    summaryPageSteps.GivenIBookTheHearing();
                    summaryPageSteps.ThenAHearingShouldBeCreated();
                    AxeAnalyze(pageName);
                    break;
                case "Booking Details":
                    ProceedToPage("Booking Confirmation");
                    WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SummaryPage.DotLoader));
                    ExtensionMethods.FindElementWithWait(Driver, BookingConfirmationPage.ViewBookingLink).Click();
                    ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.BookingConfirmedStatus, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Booking List":
                    ExtensionMethods.FindElementWithWait(Driver, Header.BookingsList, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, BookingListPage.VideoHearingsTable, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Questionnaire":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.QuestionnaireResultsButton, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.QuestionairVHTable, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Get-Audio-File":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.GetAudioFileLinkButton, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.CaseNumber, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Change-User-Password":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.ChangeUserPasswordButton, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.UserName, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Delete-User-Account":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.DeleteParticipantButton, _scenarioContext).Click();
                    AxeAnalyze(pageName);
                    break;
                case "Edit-Participant-Name":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.EditPparticipantNameButton, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.ContactEmail, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
                case "Work-Allocation":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.ManageWorkAllocation, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, WorkAllocation.EditAvailability, _scenarioContext);
                    AxeAnalyze(pageName);
                    break;
            }
        }

        private void ProceedToPage(string pageName)
        {
            CheckAccessibilityCompliance(pageName);
        }

        public void AxeAnalyze(string pageToAnalyse)
        {
            if(pageToAnalyse == PageName)
            {
                axeResult = new AxeBuilder((IWebDriver)_scenarioContext["driver"]);
                axeResult.Analyze().Violations.Where(e => e.Impact != "minor").Should().BeEmpty();
            }
        }
    }
}
