using System;
using System.Threading;
using MongoDB.Bson.Serialization.Serializers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V107.Debugger;
using OpenQA.Selenium.Support.UI;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps;

[Binding]
public class WorkAllocationManageTeamEditUser : ObjectFactory
{
    private readonly ScenarioContext _scenarioContext;
    public static String _ValidJusticeUserName = "auto_aw.CSOTestUser@hearings.reform.hmcts.net";
    public WorkAllocationManageTeamEditUser(ScenarioContext scenarioContext)
        : base(scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Then(@"I confirm User Displayed on the Page")]
    public void ThenIConfirmUserDisplayedOnThePage()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.VerifyExistJusticeUsername);
        Assert.AreEqual(_ValidJusticeUserName,
            Driver.FindElement(ManageWorkAllocationPage.VerifyExistJusticeUsername).Text);
    }

    [Then(@"I Pres edit role Icon")]
    public void ThenIPresEditRoleIcon()
    {
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRoleButton).Click();
    }

    public static void selectCsoRoleDropdown(IWebDriver driver)
    {
        var justiceUserRoleSelect = driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Text;

        if (justiceUserRoleSelect == "CSO")
        {
            driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Click();
            driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserRoleAdministration).Click();
        }
        else
        {
            driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Click();
            driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserRoleVho).Click();
        }
    }

    [Then(@"I change the Role from CSO to Administrator Or Administrator to CSO")]
    public void ThenIChangeTheRoleFromCsoToAdministratorOrAdministratorToCso()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserEditRole);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Click();
        // ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserRoleAdministration);
        var selectList = new SelectElement(Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole));
        selectList.SelectByText("Administrator");
    }

    [Then(@"I press save")]
    public void ThenIPressSave()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamEditRoleSaveButton);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRoleSaveButton).Click();
    }

    [When(@"I click on Manage Team")]
    public void WhenIClickOnManageTeam()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeam);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeam).Click();
    }

    [When(@"I Search for User ""(.*)""")]
    public void WhenISearchForUser(string p0)
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamSearchTeamMemberField);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).Click();
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).SendKeys(_ValidJusticeUserName);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchButton).Click();
    }

    [Then(@"I see Edit Role popup window")]
    public void ThenISeeEditRolePopupWindow()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamEditRolePopupWindow);
        Assert.IsTrue(Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRolePopupWindow).Displayed);
    }

    [Then(@"I see user updated confirmation on the page")]
    public void ThenISeeUserUpdatedConfirmationOnThePage()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamEditRoleConfirmationMsg);
        var ManageTeamEditRoleConfirmationExpected = "Changes saved successfully.";
        var ManageTeamEditRoleConfirmationActual =
            Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRoleConfirmationMsg).Text;
        Assert.AreEqual(ManageTeamEditRoleConfirmationExpected, ManageTeamEditRoleConfirmationActual);
    }

    [Then(@"I change the Role from CSO to Administrator")]
    public void ThenIChangeTheRoleFromCsoToAdministrator()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserEditRole);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Click();
        // ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserRoleAdministration);
        var selectList = new SelectElement(Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole));
        selectList.SelectByText("Administrator");
    }

    [Then(@"I change the Role from Administrator to CSO")]
    public void ThenIChangeTheRoleFromAdministratorToCso()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserEditRole);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Click();
        var selectList = new SelectElement(Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole));
        selectList.SelectByText("CSO");
    }

    [Then(@"I see popup message with warnings")]
    public void ThenISeePopupMessageWithWarnings()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamDeletUserPopUpWindow);
        Assert.IsTrue(Driver.FindElement(ManageWorkAllocationPage.ManageTeamDeletUserPopUpWindow).Displayed);
        Assert.AreEqual(_ValidJusticeUserName, Driver.FindElement(ManageWorkAllocationPage.VerifyDeleteUser).Text);
    }

    [Then(@"I see popup message box with correct warning")]
    public void ThenISeePopupMessageBoxWithCorrectWarning()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamRestoreUserPopUpWindow);
        Assert.IsTrue(Driver.FindElement(ManageWorkAllocationPage.ManageTeamRestoreUserPopUpWindow).Displayed);
        var verifyRestoreUserActual =
            Driver.FindElement(ManageWorkAllocationPage.VerifyManageTeamRestoreUserDetails).Text;
        var verifyRestoreUserExpected = "You have selected to restore user:\r\n" + _ValidJusticeUserName + "";
        Assert.AreEqual(verifyRestoreUserExpected, verifyRestoreUserActual);
    }
}