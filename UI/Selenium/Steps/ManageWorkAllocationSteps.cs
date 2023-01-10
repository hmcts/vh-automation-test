using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumSpecFlow.Utilities;
using System;
using NUnit.Framework;
using SeleniumSpecFlow.Utilities;
using TechTalk.SpecFlow;
using TestFramework;
using TestLibrary.Utilities;
using UISelenium.Pages;
using Util = TestLibrary.Utilities.Util;

namespace UI.Steps;

[Binding]
public class ManageWorkAllocationSteps : ObjectFactory
{
    private readonly ScenarioContext _scenarioContext;
    SelectYourHearingListSteps selectYourHearingListSteps;

    public ManageWorkAllocationSteps(ScenarioContext scenarioContext)
        : base(scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [When(@"i click on Manage Work Allocation Dashboard")]
    public void WhenIClickOnManageWorkAllocationDashboard()
    {
        SelectDashboardOption("Work-Allocation Dashboard");
        _scenarioContext.UpdatePageName("Work-Allocation Dashboard");
    }

    public void SelectDashboardOption(string optionName)
    {
        var isPageLoaded = false;
        string file;
        switch (optionName)
        {
            case "Work-Allocation Dashboard":
                ExtensionMethods.FindElementWithWait(Driver, DashboardPage.ManageWorkAllocation, _scenarioContext)
                    .Click();
                ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.EditAvailability,
                    _scenarioContext);
                break;
            case "Manage work Allocation page":
                ExtensionMethods.WaitForElementVisible(Driver,
                    ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability);
                Driver.FindElement(ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability).Click();
                break;
            case "Upload Working hours CSV File":
                ExtensionMethods.WaitForElementVisible(Driver,
                    ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability);
                Driver.FindElement(ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability).Click();
                file = Util.SetCsvFile("TestData", "Good.csv");
                ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.UploadCSVFile, _scenarioContext)
                    .SendKeys(file);
                Driver.FindElement(ManageWorkAllocationPage.UploadAvailabilityHoursButton).Click();
                break;
            case "Upload Non Availability CSV File":
                ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.UploadNonAvailabilityHours);
                Driver.FindElement(ManageWorkAllocationPage.UploadNonAvailabilityHours).Click();
                file = Util.SetCsvFile("TestData", "non Availability hours.csv");
                ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.UploadCSVFile, _scenarioContext)
                    .SendKeys(file);
                Driver.FindElement(ManageWorkAllocationPage.UploadNonAvailabilityHoursButton).Click();
                break;
        }
    }

    [Given(@"i click on Upload Workhours and non Availability")]
    public void GivenIClickOnUploadWorkhoursAndNonAvailability()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability);
        Driver.FindElement(ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability).Click();
    }

    [When(@"i click on Upload non Availability hours")]
    public void WhenIClickOnUploadNonAvailabilityHours()
    {
        SelectDashboardOption("Upload Non Availability CSV File");
        _scenarioContext.UpdatePageName("Upload Non Availability CSV File");
    }

    [Then(@"file is uploaded successfully non availability hours")]
    public void ThenFileIsUploadedSuccessfullyNonAvailabilityHours()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"i click on Upload CVS workhours")]
    public void WhenIClickOnUploadCvsWorkhours()
    {
        SelectDashboardOption("Upload Working hours CSV File");
        _scenarioContext.UpdatePageName("Upload Working hours CSV File");
    }

    [Then(@"file is uploaded successfully Working Hours")]
    public void ThenFileIsUploadedSuccessfullyWorkingHours()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.result);
        Assert.True(Driver.FindElement(ManageWorkAllocationPage.result).Displayed);
    }

    [Given(@"i click on Edit Working hours and non availability")]
    public void GivenIClickOnEditWorkingHoursAndNonAvailability()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.EditWorkingHoursNonAvailability);
        Driver.FindElement(ManageWorkAllocationPage.EditWorkingHoursNonAvailability).Click();
    }

    [Given(@"i click on Manage Team")]
    public void GivenIClickOnManageTeam()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeam);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeam).Click();
    }

    [Given(@"i click  Allocate Hearings")]
    public void GivenIClickAllocateHearings()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearings);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearings).Click();
    }
}