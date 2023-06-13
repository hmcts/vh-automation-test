namespace UI.NUnitVersion.Admin.WorkAllocation;

[Category("a11y")]
public class EditWorkHoursTests : AdminWebUiTest
{
    [Test]
    public void EditWorkHoursForExistingUser()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();

        var teamMemberUsername = "auto.vhoteamlead1@hearings.reform.hmcts.net";
        workAllocationPage.EditWorkHourForUser(teamMemberUsername, DayOfWeek.Saturday, new TimeOnly(08, 00),
            new TimeOnly(17, 00));

        Assert.Pass();
    }

    [Test]
    public void AddNonAvailableHoursForExistingUser()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();

        var teamMemberUsername = "auto.vhoteamlead1@hearings.reform.hmcts.net";

        // should be be a date far far into the future to avoid clashing with other local tests
        var startDateTime = DateTime.Today.AddDays(1).AddHours(10);
        var endDateTime = startDateTime.AddHours(1);
        workAllocationPage.AddNonAvailableDayForUser(teamMemberUsername, startDateTime, endDateTime);
    }
}