using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps;

[Binding]
public class ManageWorkAllocationSteps: ObjectFactory
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
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.Nonresult);
        Assert.True(Driver.FindElement(ManageWorkAllocationPage.Nonresult).Displayed);
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
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.Nonresult);
        Assert.True(Driver.FindElement(ManageWorkAllocationPage.Nonresult).Displayed);
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
}