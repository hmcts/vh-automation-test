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
public class WorkAllocationManageTeamEditUser :ObjectFactory
{
    private readonly ScenarioContext _scenarioContext;
    public static String _ValidJusticeUserName = "auto_aw.CSOTestUser@hearings.reform.hmcts.net";
    public WorkAllocationManageTeamEditUser(ScenarioContext scenarioContext)
        : base(scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    [Then(@"I click on Manage Team")]
    public void ThenIClickOnManageTeam()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeam);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeam).Click();
    }

    [Then(@"I Search for User ""(.*)""")]
    public void ThenISearchForUser(string p0)
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamSearchTeamMemberField);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).Click();
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).SendKeys(_ValidJusticeUserName);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchButton).Click();
    }

    [Then(@"I confirm User Displayed on the Page")]
    public void ThenIConfirmUserDisplayedOnThePage()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.VerifyExistJusticeUsername);
        Assert.AreEqual(_ValidJusticeUserName ,Driver.FindElement(ManageWorkAllocationPage.VerifyExistJusticeUsername).Text);
    }

    [Then(@"I Pres edit role Icon")]
    public void ThenIPresEditRoleIcon()
    {
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRoleButton).Click();
        
    }

    [Then(@"I see Edit Role popup window")]
    public void ThenISeeEditRolePopupWindow()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamEditRolePopupWindow);
        Assert.IsTrue(Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRolePopupWindow).Displayed);
    }
    
    public static void selectCsoRoleDropdown(IWebDriver driver )
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

    [Then(@"I chage the Role from CSO to Administrator Or Administrator to CSO")]
    public void ThenIChageTheRoleFromCsoToAdministratorOrAdministratorToCso()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserEditRole);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamJusticeUserRoleAdministration);
        var selectList = new SelectElement(Driver.FindElement(ManageWorkAllocationPage.ManageTeamJusticeUserEditRole));
        selectList.SelectByText("Administrator");
                
    }

    [Then(@"I press save")]
    public void ThenIPressSave()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamEditRoleSaveButton);
        Thread.Sleep(5000);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamEditRoleSaveButton).Click();
    }

    [Then(@"I see Sucessfull message displayed on the page")]
    public void ThenISeeSucessfullMessageDisplayedOnThePage()
    {
        ScenarioContext.StepIsPending();
    }
}