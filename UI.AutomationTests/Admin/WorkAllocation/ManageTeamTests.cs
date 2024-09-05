namespace UI.AutomationTests.Admin.WorkAllocation;

[Category("Daily")]
public class ManageTeamTests : AdminWebUiTest
{
    [Test]
    public void AddThenEditThenDeleteAndThenRestoreATeamMember()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var manageTeamTile = FeatureToggle.Instance().Dom1Enabled();
        JusticeUserManagementPage manageTeamPage =
            manageTeamTile ? dashboardPage.GoToManageTeam() : dashboardPage.GoToManageWorkAllocation();
        // var workAllocationPage = dashboardPage.GoToManageWorkAllocation();

        var newUsername = $"new.user{Guid.NewGuid():N}@automation.com";
        TestContext.Out.WriteLine("Attempting to add a new user with username: " + newUsername);
        var firstName = "Automation";
        var lastName = "Test";
        var contactTelephone = "0131 496 0881"; // generated from https://neilzone.co.uk/number/
        var roles = new List<JusticeUserRoles> {JusticeUserRoles.Vho};

        manageTeamPage.AddTeamMember(newUsername, firstName, lastName, contactTelephone, roles);
        TestContext.Out.WriteLine("Successfully added a new user with username: " + newUsername);
        var updatedRoles = new List<JusticeUserRoles> {JusticeUserRoles.VhTeamLead};
        manageTeamPage.EditTeamMember(newUsername, updatedRoles);
        TestContext.Out.WriteLine("Successfully edited a user with username: " + newUsername);
        manageTeamPage.DeleteTeamMember(newUsername);
        TestContext.Out.WriteLine("Successfully deleted a user with username: " + newUsername);
        manageTeamPage.RestoreTeamMember(newUsername);
        TestContext.Out.WriteLine("Successfully restored a user with username: " + newUsername);

        // Delete the user again
        manageTeamPage.DeleteTeamMember(newUsername);
        Assert.Pass();
    }
}