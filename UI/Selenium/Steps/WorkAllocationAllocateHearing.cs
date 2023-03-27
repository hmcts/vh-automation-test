using TechTalk.SpecFlow;
using TestFramework;
using UI.Utilities;
using NUnit.Framework;
using UI.Pages;
using System;
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