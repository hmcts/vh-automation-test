using TechTalk.SpecFlow;
using TestFramework;
using UI.Utilities;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;
using System;
using OpenQA.Selenium.DevTools;

namespace UI.Steps;

[Binding]
public class WorkAllocationAllocateHearing :ObjectFactory

{
    private readonly ScenarioContext _scenarioContext;
    
    public WorkAllocationAllocateHearing(ScenarioContext scenarioContext)
        : base(scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    [Then(@"I click on Allocate Hearing")]
    public void ThenIClickOnAllocateHearing()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingsTab);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsTab).Click();
    }

    [Then(@"I Select Date Range")]
    public void ThenISelectDateRange()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingsFromDate);
        var allocateHearingtoday = DateTime.Today.ToString("dd/MM/yyyy");
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsFromDate).SendKeys(allocateHearingtoday);
    }

    [Then(@"I press Search")]
    public void ThenIPressSearch()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingSearchButton);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearchButton).Click();
    }

    [Then(@"I Select Allocate To User ""(.*)""")]
    public void ThenISelectAllocateToUser(string p0)
    {
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingCSOSelectList); 
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).Click();
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).SendKeys("userdonot");
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingCsoSelect);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCsoSelect).Click();
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearchButton).Click();
        
    }

    [Then(@"I Select First and Second Hearing")]
    public void ThenISelectFirstAndSecondHearing()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingSelectFirstCase);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSelectFirstCase).Click();
      //  Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSelectSecondCase).Click();
    }

    [Then(@"I click confirm button")]
    public void ThenIClickConfirmButton()
    {
        
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingConfirmButton);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingConfirmButton).Click();
    }

    [Then(@"I See Hearing have been updated message")]
    public void ThenISeeHearingHaveBeenUpdatedMessage()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.VerifyAllocateHearingConfirmMsg);
        var VerifyHearingupdatedActual = Driver.FindElement(ManageWorkAllocationPage.VerifyAllocateHearingConfirmMsg).Text;
        var VerifyHearingupdatedExpected = "Hearings have been updated.";
        Assert.AreEqual(VerifyHearingupdatedActual, VerifyHearingupdatedExpected);
        
    }

    [Then(@"I select manage Team and Delete User and Restore user to unallocate Hearing")]
    public void ThenISelectManageTeamAndDeleteUserAndRestoreUserToUnallocateHearing()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeam);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeam).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamSearchTeamMemberField);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).Click();
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).SendKeys("userdonot");
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchButton).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamDeleteUser);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamDeleteUser).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamDeletUserPopUpWindow);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamDeleteUserYesButton).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamRestoreUserButton);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamRestoreUserButton).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamRestoreUserYesButton);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamRestoreUserYesButton).Click();
        var VerifyRestoreUserActual =
            Driver.FindElement(ManageWorkAllocationPage.VerifyManageTeamRestoreUserConfirmation).Text;
        var VerifyRestoreUserExpected = "Changes saved successfully.";
        Assert.AreEqual(VerifyRestoreUserExpected, VerifyRestoreUserActual);

    }
}