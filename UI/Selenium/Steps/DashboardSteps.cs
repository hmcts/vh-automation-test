using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Dashboard page
    ///</summary>
    public class DashboardSteps: ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        SelectYourHearingListSteps selectYourHearingListSteps;
        public DashboardSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I select book a hearing")]
        public void GivenISelectBookAHearing()
        {
            SelectDashboardOption("Book a video hearing");
            _scenarioContext.UpdatePageName("Book a video hearing");
        }

        public void SelectDashboardOption(string optionName)
        {
            var isPageLoaded = false;
            switch(optionName)
            {
                case "Book a video hearing":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.BookHearingButton, _scenarioContext).Click();
                    isPageLoaded=IsHeardingDetailsPageLoaded();
                    isPageLoaded.Should().BeTrue($"cannot load {optionName} page");
                    break;
                case "Get audio file link":
                    ExtensionMethods.WaitForElementVisible(Driver, DashboardPage.GetAudioFileLinkButton);
                    Driver.FindElement(DashboardPage.GetAudioFileLinkButton).Click();
                    ExtensionMethods.WaitForElementVisible(Driver, GetAudioFilePage.CaseNumberInput);
                    break;
                case "Work-Allocation Dashboard":
                    ExtensionMethods.FindElementWithWait(Driver, DashboardPage.ManageWorkAllocation, _scenarioContext).Click();
                    ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.EditAvailability, _scenarioContext);
                    break;
                case "Manage work Allocation page":
                    ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability);
                    Driver.FindElement(ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability).Click();
                    break;
                case "Upload Working hours CSV File":
                    ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability);
                    Driver.FindElement(ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability).Click();
                    var file = Util.SetCsvFile("TestData", "Good.csv");
                    ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.UploadCSVFile, _scenarioContext).SendKeys(file);
                    Driver.FindElement(ManageWorkAllocationPage.UploadAvailabilityHoursButton).Click();
                    break;
                case "Upload Non Availability CSV File":
                    ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.UploadNonAvailabilityHours);
                    //Driver.FindElement(ManageWorkAllocationPage.UploadNonAvailabilityHours).Click();
                    file = Util.SetCsvFile("TestData", "NonAvailabilityHours.csv");
                    ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.UploadNonAvailabilityHours, _scenarioContext).SendKeys(file);
                    Driver.FindElement(ManageWorkAllocationPage.UploadNonAvailabilityHoursButton).Click();
                    break;
            }
        }

        public bool IsHeardingDetailsPageLoaded()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.DefaultElementWait));
                wait.Until(ExpectedConditions.ElementIsVisible(HearingDetailsPage.CaseNumber));
                return true;
            }
            catch
            {
                return false;
            }
        }

        [When(@"selects hearing venue in the venue list")]
        public void WhenSelectsHearingVenueInTheVenueList()
        {
            selectYourHearingListSteps = new SelectYourHearingListSteps(_scenarioContext);
            selectYourHearingListSteps.SelectVenue("Birmingham Civil and Family Justice Centre");
            selectYourHearingListSteps.WhenIClickOnViewHearings();
        }
        
        [When(@"i click on Manage Work Allocation Dashboard")]
        public void GivenIClickOnManageWorkAllocation()
        {
            SelectDashboardOption("Work-Allocation Dashboard");
            _scenarioContext.UpdatePageName("Work-Allocation Dashboard");
           
        }


        [When(@"i click on Upload CVS workhours")]
        public void WhenIClickOnUploadCvsWorkhours()
        {
            SelectDashboardOption("Upload Working hours CSV File");
            _scenarioContext.UpdatePageName("Upload Working hours CSV File");
        }

        [When(@"i click on Upload non Availability hours")]
        public void WhenIClickOnUploadNonAvailabilityHours()
        {
            SelectDashboardOption("Upload Non Availability CSV File");
            _scenarioContext.UpdatePageName("Upload Non Availability CSV File");
        }
    }
}
