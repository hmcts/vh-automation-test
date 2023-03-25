using System;
using System.Text;
using NUnit.Framework;
using RandomNameGenerator;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps;

[Binding]
public class WorkAllocationManageTeam : ObjectFactory
{
    private readonly ScenarioContext _scenarioContext;
    public static String _justiceUserName = GetRandomJusticeUserName();

    public WorkAllocationManageTeam(ScenarioContext scenarioContext)
        : base(scenarioContext)
    {
        _scenarioContext = scenarioContext;
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
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamSearchTeamMemberField);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).Click();
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchTeamMemberField).SendKeys(_justiceUserName);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamSearchButton).Click();
    }


    [Then(@"I see no user found message and add new user button")]
    public void ThenISeeNoUserFoundMessageAndAddNewUserButton()
    {
        var errorMsgTextUserNotFoundActual = Driver.FindElement(ManageWorkAllocationPage.ManageTeamNouserErrorMsg).Text;
        var errorMsgTextUserNotFoundExpect =
            "No users matching this search criteria were found. Please check the search and try again. Or, add the team member.";
        Assert.AreEqual(errorMsgTextUserNotFoundActual, errorMsgTextUserNotFoundExpect);
    }

    [Then(@"I click add new user")]
    public void ThenIClickAddNewUser()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamAddNewTeamMember);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamAddNewTeamMember).Click();
    }

    [Then(@"I see new pop up window - Add a justice user")]
    public void ThenISeeNewPopUpWindowAddAJusticeUser()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamAddJusticeUserPopUp);
        var popUpWindowAddJusticeUser =
            Driver.FindElement(ManageWorkAllocationPage.ManageTeamAddJusticeUserPopUp).Displayed;
        Assert.IsTrue(popUpWindowAddJusticeUser);
    }

    [Then(@"i fill in all details of new user with correct UK number and valid email address")]
    public void ThenIFillInAllDetailsOfNewUserWithCorrectUkNumberAndValidEmailAddress()
    {
        _scenarioContext.UpdatePageName("Add a Justice User");
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AddJusticeUserID);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserID).SendKeys(_justiceUserName);
        var randomFirstName = NameGenerator.GenerateFirstName(Gender.Male);
        var randomLastName = NameGenerator.GenerateLastName();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AddJusticeUserSaveButton);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserFirstName).SendKeys(randomFirstName);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserLastName).SendKeys(randomLastName);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserContactNumber)
            .SendKeys(GenerateRandonUkPhoneNumber());
    }

    [Then(@"I save changes")]
    public void ThenISaveChanges()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AddJusticeUserSaveButton);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserSaveButton).Click();
    }

    [Then(@"i see save successful message and user details")]
    public void ThenISeeSaveSuccessfulMessageAndUserDetails()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.VerifyJusticeUsername);
        Assert.AreEqual(_justiceUserName, Driver.FindElement(ManageWorkAllocationPage.VerifyJusticeUsername).Text);
    }


    [Then(@"I delete User")]
    public void ThenIDeleteUser()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamDeleteUser);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamDeleteUser).Click();
    }

    [Then(@"I see popup message with warnings")]
    public void ThenISeePopupMessageWithWarnings()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamDeletUserPopUpWindow);
        Assert.IsTrue(Driver.FindElement(ManageWorkAllocationPage.ManageTeamDeletUserPopUpWindow).Displayed);
        Assert.AreEqual(_justiceUserName, Driver.FindElement(ManageWorkAllocationPage.VerifyDeleteUser).Text);
    }

    [Then(@"I Click Yes,proceed button")]
    public void ThenIClickYesProceedButton()
    {
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamDeleteUserYesButton).Click();
    }


    [Then(@"I confirm user has been deleted with option to restore")]
    public void ThenIConfirmUserHasBeenDeletedWithOptionToRestore()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.VerifyManageTeamDeleteUser);
        var userDeleted = Driver.FindElement(ManageWorkAllocationPage.VerifyManageTeamDeleteUser).Text;
        Assert.AreEqual("Deleted", userDeleted);
    }

    [Then(@"I click restore team member icon")]
    public void ThenIClickRestoreTeamMemberIcon()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamRestoreUserButton);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamRestoreUserButton).Click();
    }

    [Then(@"I see popup message box with correct warning")]
    public void ThenISeePopupMessageBoxWithCorrectWarning()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamRestoreUserPopUpWindow);
        Assert.IsTrue(Driver.FindElement(ManageWorkAllocationPage.ManageTeamRestoreUserPopUpWindow).Displayed);
        var verifyRestoreUserActual =
            Driver.FindElement(ManageWorkAllocationPage.VerifyManageTeamRestoreUserDetails).Text;
        var verifyRestoreUserExpected = "You have selected to restore user:\r\n" + _justiceUserName + "";
        Assert.AreEqual(verifyRestoreUserExpected, verifyRestoreUserActual);
    }

    [Then(@"I click Yes,proceed button to restore user")]
    public void ThenIClickYesProceedButtonToRestoreUser()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.ManageTeamRestoreUserYesButton);
        Driver.FindElement(ManageWorkAllocationPage.ManageTeamRestoreUserYesButton).Click();
    }

    [Then(@"I confirm user has been restored\.")]
    public void ThenIConfirmUserHasBeenRestored()
    {
        var VerifyRestoreUserActual =
            Driver.FindElement(ManageWorkAllocationPage.VerifyManageTeamRestoreUserConfirmation).Text;
        var VerifyRestoreUserExpected = "Changes saved successfully.";
        Assert.AreEqual(VerifyRestoreUserExpected, VerifyRestoreUserActual);
    }


    public static String GetRandomJusticeUserName()
    {
        Random randomGenerator = new Random();
        var randomInt = randomGenerator.NextInt64(1000);
        var manageTeamUserName = "auto.VH.TestUser" + randomInt + "@hearings.hmcts.net";
        return manageTeamUserName;
    }

    private static string GenerateRandonUkPhoneNumber()

    {
        var sb = new StringBuilder();
        sb.Append("+44(0)744");

        var rnd = new Random(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < 7; i++)
        {
            sb.Append(rnd.Next(0, 6).ToString());
        }

        return sb.ToString();
    }
}