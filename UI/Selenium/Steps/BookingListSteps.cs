using FluentAssertions;
using SeleniumSpecFlow.Utilities;
using TechTalk.SpecFlow;
using SeleniumSpecFlow.Steps;
using UI.Utilities;
using UI.Model;
using TestFramework;
using UISelenium.Pages;
using System;
using NUnit.Framework;

namespace UI.Steps
{
     internal class BookingListSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        public string username = "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net";
        private LoginPageSteps loginSteps;
        private DashboardSteps dashboardSteps;
        private HearingScheduleSteps hearingScheduleSteps;
        private CreateHearingDetails createHearingDetails;
        ParticipantsSteps participantsSteps;
        HearingAssignJudgeSteps hearingAssignJudgeSteps;
        VideoAccessSteps videoAccessSteps;
        OtherInformationSteps otherInformationSteps;
        SummaryPageSteps summaryPageSteps;
        Hearing _hearing;

        public BookingListSteps(ScenarioContext scenarioContext)
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

        [Given(@"I have a booked hearing")]
        public void GivenIHaveABookedHearing()
        {

            loginSteps.GivenILogInAs(username);
            dashboardSteps.GivenISelectBookAHearing();
            createHearingDetails.GivenIWantToCreateAHearingWithCaseDetails(StepsHelper.Set.HearingDetailsData());
            _hearing = (Hearing)_scenarioContext["Hearing"];
            hearingScheduleSteps = new HearingScheduleSteps(_scenarioContext);
            hearingScheduleSteps.GivenTheHearingHasTheFollowingScheduleDetails(StepsHelper.Set.HearingScheduleData());
            hearingAssignJudgeSteps = new HearingAssignJudgeSteps(_scenarioContext);
            hearingAssignJudgeSteps.GivenIWantToAssignAJudgeWithCourtroomDetails(StepsHelper.Set.JudgeData());
            participantsSteps.GivenIWantToCreateAHearingFor(StepsHelper.Set.ParticipantsData());
            videoAccessSteps.GivenWithVideoAccessPointsDetails(StepsHelper.Set.VideoAccessPointData());
            otherInformationSteps.GivenISetAnyOtherInformation(StepsHelper.Set.SetOtherInfoData());
            summaryPageSteps.GivenIBookTheHearing();
            summaryPageSteps.ThenAHearingShouldBeCreated();
        }

        [When(@"I navigate to booking list page")] 
        public void INavigateToBookingListPage()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            ExtensionMethods.FindElementWithWait(Driver, Header.BookingsList, _scenarioContext).Click();
            ExtensionMethods.WaitForElementVisible(Driver, BookingListPage.HearingDateTitle);
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDateTitle).Displayed.Should().BeTrue();
        }

        [Then(@"The booking should contain expected values")]
        public void ThenTheBookingShouldContainExpectedValues()
        {
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRow).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.Case.CaseName)).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.Case.CaseNumber)).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.HearingSchedule.HearingVenue)).Displayed.Should().BeTrue();
        }

        [When(@"the VHO search for the booking by case number")]
        public void WhenTheVHOSearchForTheBookingByCaseNumber()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchPanelButton,_scenarioContext).Click();
            Driver.FindElement(BookingListPage.SearchCaseTextBox).SendKeys(_hearing.Case.CaseNumber);
            Driver.FindElement(BookingListPage.SearchButton).Click();
        }

        [Then(@"the booking is retrieved")]
        public void ThenTheBookingIsRetrieved()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.BookingConfirmedStatus, _scenarioContext);
            Driver.FindElements(BookingListPage.HearingDetailsRow).Count.Should().Be(1);
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.Case.CaseName)).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.Case.CaseNumber)).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.HearingSchedule.HearingVenue)).Displayed.Should().BeTrue();
        }

        [Then(@"VHO selects booking")]
        public void ThenVHOSelectsBooking()
        {
            var element=ExtensionMethods.FindElementWithWait(Driver, BookingListPage.HearingSelectionSpecificRow(_hearing.Case.CaseNumber), _scenarioContext);
            element.Click();
        }

        [Then(@"the VHO is on the Booking Details page")]
        public void ThenTheVHOIsOnTheBookingDetailsPage()
        {
             ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.SpecificBookingConfirmedStatus(_hearing.Case.CaseNumber), _scenarioContext).Displayed.Should().BeTrue();
        }

        [When(@"I search for case number")]
        public void WhenISearchForCaseNumber()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchPanelButton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchCaseTextBox, _scenarioContext).SendKeys(_hearing.Case.CaseNumber);
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchButton, _scenarioContext).Click();

        }

        [When(@"I copy telephone participant link")]
        public void WhenICopyTelephoneParticipantLink()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.ConfirmedButton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.TelephoneParticipantLink, _scenarioContext).Click();
            _hearing.BookingList.TelephoneParticipantLink = new TextCopy.Clipboard().GetText();
        }

        [Then(@"telephone participant link should be copied")]
        public void ThenTelephoneParticipantLinkShouldBeCopied()
        {
            Assert.IsTrue(_hearing.BookingList.TelephoneParticipantLink.Contains("+448000488500"), "Phone verified");
        }

        [When(@"I copy video participant link")]
        public void WhenICopyVideoParticipantLink()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.ConfirmedButton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.VideoParticipantLink, _scenarioContext).Click();
            _hearing.BookingList.VideoParticipantLink = new TextCopy.Clipboard().GetText();
        }

        [Then(@"video participant link should be copied")]
        public void ThenVideoParticipantLinkShouldBeCopied()
        {
            Assert.IsTrue(_hearing.BookingList.VideoParticipantLink.Contains(".hearings.reform.hmcts.net"), "Video link verification failed :" + _hearing.BookingList.VideoParticipantLink);
        }
    }
}