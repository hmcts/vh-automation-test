namespace UI.NUnitVersion.Admin;

public class UploadWorkHourTests : AdminWebUiTest
{
    [Test]
    public void UploadValidWorkHours()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(Username, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();
        workAllocationPage.UploadWorkHoursFile(Path.Join("TestData", "GoodWorkHours.csv"));
        workAllocationPage.WaitForFileUploadSuccessMessage();
        Assert.Pass();
    }

    [Test]
    public void UploadValidNonAvailabilityHours()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(Username, EnvConfigSettings.UserPassword);

        var workAllocationPage = dashboardPage.GoToManageWorkAllocation();
        workAllocationPage.UploadNonWorkHoursFile(Path.Join("TestData", "GoodNonAvailabilityHours.csv"));
        workAllocationPage.WaitForFileUploadSuccessMessage();
        Assert.Pass();
    }
}