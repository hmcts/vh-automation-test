using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;
using System;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;

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


    [Then(@"file is uploaded successfully Working Hours")]
    public void ThenFileIsUploadedSuccessfullyWorkingHours()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.TeamWorkingHoursUploadedSuccessfully);
        Assert.True(Driver.FindElement(ManageWorkAllocationPage.TeamWorkingHoursUploadedSuccessfully).Displayed);
    }

    [Given(@"i click on Upload Workhours and non Availability")]
    public void GivenIClickOnUploadWorkhoursAndNonAvailability()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability);
        Driver.FindElement(ManageWorkAllocationPage.UploadWorkingHoursOrNonAvailability).Click();
    }

    [Then(@"file is uploaded successfully non availability hours")]
    public void ThenFileIsUploadedSuccessfullyNonAvailabilityHours()
    {
        ExtensionMethods.WaitForElementVisible(Driver,
            ManageWorkAllocationPage.TeamNonAvailabilityHoursUploadedSuccessfully);
        var teamNonAvailabilityHoursUploadedSuccessfully = "Team non-availability hours uploaded successfully";
        var getTextNonAvailabilityHoursSuccess =
            Driver.FindElement(ManageWorkAllocationPage.TeamNonAvailabilityHoursUploadedSuccessfully).Text;
        Assert.AreEqual(teamNonAvailabilityHoursUploadedSuccessfully, getTextNonAvailabilityHoursSuccess);
    }

    [Given(@"i click on Edit Working hours and non availability")]
    [When(@"i click on Edit Working hours and non availability")]
    public void GivenIClickOnEditWorkingHoursAndNonAvailability()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.EditWorkingHoursNonAvailability);
        Driver.FindElement(ManageWorkAllocationPage.EditWorkingHoursNonAvailability).Click();
    }

    [Given(@"i click on Manage Team")]
    [When(@"i click on Manage Team")]
    public void GivenIClickOnManageTeam()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeam);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeam).Click();
    }

    [Given(@"i click  Allocate Hearings")]
    [When(@"i click  Allocate Hearings")]
    public void GivenIClickAllocateHearings()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearings);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearings).Click();
    }

    [Then(@"i select Edit Working hour radio Button")]
    public void ThenISelectEditWorkingHourRadioButton()
    {
        //ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.EditWorkinghoursRadioButton);
        Driver.FindElement(ManageWorkAllocationPage.EditWorkinghoursRadioButton).Click();
    }

    [Then(@"Search for team member")]
    public void ThenSearchForTeamMember()
    {
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).Click();
    }

    [Given(@"i Click on Edit working hours button")]
    public void GivenIClickOnEditWorkingHoursButton()
    {
        Driver.FindElement(ManageWorkAllocationPage.EditWorkinghoursRadioButton).Click();
    }

    [Given(@"Search team members")]
    public void GivenSearchTeamMembers()
    {
        Driver.FindElement(ManageWorkAllocationPage.SearchFieldUsername)
            .SendKeys("Phil.debrah@hearings.reform.hmcts.net");
        Driver.FindElement(ManageWorkAllocationPage.SearchButton).Click();
    }

    [When(@"Search for team member")]
    public void WhenSearchForTeamMember()
    {
        Driver.FindElement(ManageWorkAllocationPage.SearchForTeamMember)
            .SendKeys("Phil.debrah@hearings.reform.hmcts.net");
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchButton).Click();
    }


    [When(@"I search Allocate hearings Date Range start")]
    public void WhenISearchAllocateHearingsDateRangeStart()
    {
        var startDate = DateTime.Now.ToString("dd/MM/yyyy");
        var endDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
        //ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.HearingRangeStartDate, _scenarioContext).Click();
        ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.HearingRangeStartDate, _scenarioContext)
            .SendKeys(startDate);
        ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.HearingRangeEndDate, _scenarioContext)
            .SendKeys(endDate);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearch).Click();
    }

    [When(@"i select Allocated CSO")]
    public void WhenISelectAllocatedCso()
    {
        Driver.FindElement(ManageWorkAllocationPage.AlloctatedCSO).Click();
        ExtensionMethods.FindElementWithWait(Driver, ManageWorkAllocationPage.AlloctatedCSOList, _scenarioContext)
            .Click();
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearch).Click();
    }

    [Then(@"Team working hours uploaded successfully")]
    public void ThenTeamWorkingHoursUploadedSuccessfully()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.TeamWorkingHoursUploadedSuccessfullyM);
        var teamWorkingHoursUploadedSuccessfully = "Team working hours uploaded successfully";
        var getTextWorkingHoursFileUpalodSucess =
            Driver.FindElement(ManageWorkAllocationPage.TeamWorkingHoursUploadedSuccessfullyM).Text;
        Assert.AreEqual(teamWorkingHoursUploadedSuccessfully, getTextWorkingHoursFileUpalodSucess);
    }

    [Then(@"i click manage team")]
    public void ThenIClickManageTeam()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeam);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeam).Click();
    }

    [Then(@"I search for new user")]
    public void ThenISearchForNewUser()
    {
       
    }
}