namespace UI.NUnitVersion.Admin.WorkAllocation;

public class UploadWorkHourTests : AdminWebUiTest
{
    [Test]
    [Category("a11y")]
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
}