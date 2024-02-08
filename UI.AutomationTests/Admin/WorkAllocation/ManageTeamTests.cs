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

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();

        var newUsername = $"new.user{Guid.NewGuid():N}@automation.com";
        TestContext.WriteLine("Attempting to add a new user with username: " + newUsername);
        var firstName = "Automation";
        var lastName = "Test";
        var contactTelephone = "0131 496 0881"; // generated from https://neilzone.co.uk/number/
        var roles = new List<JusticeUserRoles> {JusticeUserRoles.Vho};

        workAllocationPage.AddTeamMember(newUsername, firstName, lastName, contactTelephone, roles);
        TestContext.WriteLine("Successfully added a new user with username: " + newUsername);
        var updatedRoles = new List<JusticeUserRoles> {JusticeUserRoles.VhTeamLead};
        workAllocationPage.EditTeamMember(newUsername, updatedRoles);
        TestContext.WriteLine("Successfully edited a user with username: " + newUsername);
        workAllocationPage.DeleteTeamMember(newUsername);
        TestContext.WriteLine("Successfully deleted a user with username: " + newUsername);
        workAllocationPage.RestoreTeamMember(newUsername);
        TestContext.WriteLine("Successfully restored a user with username: " + newUsername);

        // Delete the user again
        workAllocationPage.DeleteTeamMember(newUsername);
        Assert.Pass();
    }
}