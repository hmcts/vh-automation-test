namespace UI.NUnitVersion.Admin.WorkAllocation;

public class AllocateHearingTests : AdminWebUiTest
{
    [Test]
    public void AllocateAJusticeUserToAHearing()
    {
        // should I book a hearing first?
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var manageWorkAllocationPage = dashboardPage.GoToManageWorkAllocation();
        manageWorkAllocationPage.AllocateJusticeUserToHearing();
        
        Assert.Pass();
    }
}