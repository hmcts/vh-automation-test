namespace UI.AutomationTests.Admin.WorkAllocation;


public class WorkHourTests : AdminWebUiTest
{
    [Test]
    [Order(1)]
    [Category("admin")]
    public void UploadValidWorkHours()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();
        workAllocationPage.UploadWorkHoursFile(Path.Join("TestData", "GoodWorkHours.csv"));
        Assert.Pass();
    }

    [Test]
    [Order(1)]
    [Category("admin")]
    public void UploadValidNonAvailabilityHours()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();
        workAllocationPage.UploadNonWorkHoursFile(Path.Join("TestData", "GoodNonAvailabilityHours.csv"));
        Assert.Pass();
    }
    
    [Test]
    [Order(2)]
    [Category("admin")]
    public async Task EditWorkHoursForExistingUser()
    {
        var teamMemberUsername = WorkAllocationTestData.JusticeUserUsername;
        await CreateVhTeamLeaderJusticeUserIfNotExist(teamMemberUsername);
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();

        
        workAllocationPage.EditWorkHourForUser(teamMemberUsername, DayOfWeek.Saturday, new TimeOnly(08, 00),
            new TimeOnly(17, 00));

        Assert.Pass();
    }

    [Test]
    [Order(2)]
    [Category("admin")]
    public async Task AddNonAvailableHoursForExistingUser()
    {
        var teamMemberUsername = WorkAllocationTestData.JusticeUserUsername;
        await CreateVhTeamLeaderJusticeUserIfNotExist(teamMemberUsername);
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();

        // should be be a date far far into the future to avoid clashing with other local tests
        var startDateTime = DateTime.Today.AddDays(1).AddHours(10);
        var endDateTime = startDateTime.AddHours(1);
        workAllocationPage.AddNonAvailableDayForUser(teamMemberUsername, startDateTime, endDateTime);
        
        Assert.Pass();
    }

    protected override async Task CleanUp()
    {
        var username = WorkAllocationTestData.JusticeUserUsername;
        var nonAvailableHours = await BookingsApiClient.GetVhoNonAvailabilityHoursAsync(username);
        foreach (var nonAvailability in nonAvailableHours.Where(x=> x.StartTime > DateTime.Today))
        {
            await BookingsApiClient.DeleteVhoNonAvailabilityHoursAsync(username, nonAvailability.Id);
        }
        await TestContext.Out.WriteLineAsync($"Removed justice user {username} non availability hours");
    }
}