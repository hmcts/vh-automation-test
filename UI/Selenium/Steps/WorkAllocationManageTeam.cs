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
    public static String _randomFirstName = NameGenerator.GenerateFirstName(Gender.Male);
    public static String _randomLastName = NameGenerator.GenerateLastName();

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
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AddJusticeUserSaveButton);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserFirstName).SendKeys(_randomFirstName);
        Driver.FindElement(ManageWorkAllocationPage.AddJusticeUserLastName).SendKeys(_randomLastName);
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
        var randomInt = randomGenerator.NextInt64(100000);
        var manageTeamUserName = "auto.VH.TestUser" + randomInt + "@hearings.hmcts.net";
        return manageTeamUserName;
    }

    [Then(@"I click on Allocate Hearing")]
    public void ThenIClickOnAllocateHearing()
    {
        Driver.Navigate().Refresh();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingsTab);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsTab).Click();
    }

    [Then(@"I Select Date Range")]
    public void ThenISelectDateRange()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingsFromDate);
        var allocateHearingToday = DateTime.Today.ToString("dd/MM/yyyy");
        var allocateHearingFuture = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy");
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsFromDate).SendKeys(allocateHearingToday);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsEndDate).SendKeys(allocateHearingFuture);
    }

    [Then(@"I press Search")]
    public void ThenIPressSearch()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingSearchButton);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearchButton).Click();
    }

   
    [Then(@"I Select First and Second Hearing")]
    public void ThenISelectFirstAndSecondHearing()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingSelectFirstCase);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSelectFirstCase).Click();
        
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
        var VerifyHearingupdatedActual =
            Driver.FindElement(ManageWorkAllocationPage.VerifyAllocateHearingConfirmMsg).Text;
        var VerifyHearingupdatedExpected = "Hearings have been updated.";
        Assert.AreEqual(VerifyHearingupdatedActual, VerifyHearingupdatedExpected);
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

    [Then(@"I Select Allocate To User")]
    public void ThenISelectAllocateToUser()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingAllocateToCSO);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingAllocateToCSO).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingAllocateToCSO);
        //Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).Click();
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingAllocateToCSO).SendKeys(_randomFirstName);
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingCsoSelect);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCsoSelect).Click();
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearchButton).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingPageClick);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingPageClick).Click();

    }
}