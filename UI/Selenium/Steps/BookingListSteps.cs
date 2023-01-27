using FluentAssertions;
using TechTalk.SpecFlow;
using UI.Utilities;
using UI.Model;
using TestFramework;
using System;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using UI.Pages;
using UI.Pages.PageElements;

namespace UI.Steps
{
    ///<summary>
    /// Steps class for Bookings List
    ///</summary>
    [Binding]
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

        [Given(@"I have booked a hearing in next (\d+) minutes")]
        public void GivenIHaveABookedHearingInNextMinutes(int min)
        {
            loginSteps.GivenILogInAs(username);
            dashboardSteps.GivenISelectBookAHearing();
            createHearingDetails.GivenIWantToCreateAHearingWithCaseDetails(StepsHelper.Set.HearingDetailsData());
            _hearing = (Hearing)_scenarioContext["Hearing"];
            hearingScheduleSteps = new HearingScheduleSteps(_scenarioContext);
            hearingScheduleSteps.GivenTheHearingHasTheScheduleDetails(StepsHelper.Set.HearingScheduleData(), min);
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
            ExtensionMethods.FindElementWithWait(Driver, Header.BookingsList, _scenarioContext).Click();
            ExtensionMethods.WaitForElementVisible(Driver, BookingListPage.HearingDetailsRow);
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRow).Displayed.Should().BeTrue();
        }

        [Then(@"The booking should contain expected values")]
        public void ThenTheBookingShouldContainExpectedValues()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRow).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.Case.CaseName)).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.Case.CaseNumber)).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementEnabledWithWait(Driver, BookingListPage.HearingDetailsRowSpecific(_hearing.HearingSchedule.HearingVenue)).Displayed.Should().BeTrue();
        }

        [When(@"the VHO search for the booking by case number")]
        public void WhenTheVHOSearchForTheBookingByCaseNumber()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchPanelButton, _scenarioContext).Click();
            Driver.FindElement(BookingListPage.SearchCaseTextBox).SendKeys(_hearing.Case.CaseNumber);
            Driver.FindElement(BookingListPage.SearchButton).Click();
        }

        [When(@"the VHO search for the booking by venue '(.*)'")]
        public void WhenTheVHOSearchForTheBookingByVenue(string venue)
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchPanelButton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.VenueListbox, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.VenueCheckbox(venue), _scenarioContext).Click();
            Driver.FindElement(BookingListPage.SearchButton).Click();
        }

        [When(@"the VHO search for the booking by date")]
        public void WhenTheVHOSearchForTheBookingByDate()
        {
            var currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchPanelButton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.StartDate, _scenarioContext).SendKeys(currentDate);
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.EndDate, _scenarioContext).SendKeys(currentDate); ;
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

        [When(@"the VHO scrolls to the hearing")]
        public void WhenTheVHOScrollsToTheHearing()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var isFound = false;
            Actions actions = new Actions(Driver);
            int attempts = 0;
            while (!isFound && attempts <= 20)
            {
                isFound = Driver.FindElement(OpenQA.Selenium.By.TagName("body")).Text.Contains(_hearing.Case.CaseNumber);
                if (!isFound)
                {
                    actions = new Actions(Driver);
                    actions.SendKeys(OpenQA.Selenium.Keys.End).Perform();
                    actions.SendKeys(OpenQA.Selenium.Keys.PageUp).Perform();
                }
                attempts++;
            }
        }

        [Then(@"VHO selects booking")]
        public void ThenVHOSelectsBooking()
        {
            Driver.RetryClick(BookingListPage.HearingSelectionSpecificRow(_hearing.Case.CaseNumber), _scenarioContext, TimeSpan.FromSeconds(Config.DefaultElementWait));
        }

        [Then(@"the VHO is on the Booking Details page")]
        public void ThenTheVHOIsOnTheBookingDetailsPage()
        {
             ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.SpecificBookingConfirmedStatus(_hearing.Case.CaseNumber), _scenarioContext).Displayed.Should().BeTrue();
        }

        [When(@"I search for case number")]
        public void WhenISearchForCaseNumber()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
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
            Assert.IsTrue(_hearing.BookingList.TelephoneParticipantLink.Contains(Config.UKConferencePhoneNumber), "Phone verified");
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
            Assert.IsTrue(_hearing.BookingList.VideoParticipantLink.Contains(Config.VideoUrl+"/quickjoin/"), "Video link verification failed :" + _hearing.BookingList.VideoParticipantLink);
        }

        [When(@"the VHO cancels the hearing for the reason '(.*)'")]
        public void WhenTheVHOCancelsTheHearing(string reason)
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.CancelBookingButton, _scenarioContext).Click();
            var selectElement = new SelectElement(ExtensionMethods.WaitForDropDownListItems(Driver, BookingDetailsPage.CancelReason));
            selectElement.SelectByText(reason);
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.ConfirmCancelButton, _scenarioContext).Click();
        }

        [Then(@"the VHO sees the hearing is cancelled")]
        public void ThenTheVHOSeesTheHearingIsCancelled()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.SpecificBookingCancelledStatus(_hearing.Case.CaseNumber), _scenarioContext).Displayed.Should().BeTrue();
        }

        [When(@"the VHO search for the booking by case type '(.*)'")]
        public void WhenTheVHOSearchForTheBookingByCaseType(string caseType)
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.SearchPanelButton, _scenarioContext).Click();
            foreach(var type in caseType.Split(','))
            {
                var _type = type.Trim();
                ExtensionMethods.FindElementWithWait(Driver, BookingListPage.CaseTypes, _scenarioContext).SendKeys(_type);
                ExtensionMethods.WaitForElementVisible(Driver, BookingListPage.CaseTypeCheckbox(_type));
                ExtensionMethods.FindElementWithWait(Driver, BookingListPage.CaseTypeCheckbox(_type), _scenarioContext).Click();
            }
            
            ExtensionMethods.FindElementWithWait(Driver, BookingListPage.VenueListbox, _scenarioContext).Click();
            Driver.FindElement(BookingListPage.SearchButton).Click();
        }

        [Then(@"VHO selects any booking")]
        public void ThenVHOSelectsAnyBooking()
        {
            ExtensionMethods.WaitForElementVisible(Driver, BookingListPage.HearingDetailsRow);
            Driver.FindElement(BookingListPage.HearingDetailsRow).Click();
        }

        [Then(@"the VHO can see Booking Details page")]
        public void ThenTheVHOCanSeeBookingDetailsPage()
        {
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.BookingConfirmedStatus, _scenarioContext).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.EditBookingButton, _scenarioContext).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.ParticipantDetails, _scenarioContext).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.CourtRoomAddress, _scenarioContext).Displayed.Should().BeTrue();
            ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.HearingStart, _scenarioContext).Displayed.Should().BeTrue();
        }
    }
}