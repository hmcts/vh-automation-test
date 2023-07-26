namespace UI.NUnitVersion.Admin.WorkAllocation;

public class AllocateHearingTests : AdminWebUiTest
{
    [Test]
    public void AllocateAJusticeUserToAHearing()
    {
        // should I book a hearing first?
        // book a hearing via the API directly
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var manageWorkAllocationPage = dashboardPage.GoToManageWorkAllocation();
        manageWorkAllocationPage.AllocateJusticeUserToHearing(justiceUserDisplayName:"Auto VHoteamleader1", justiceUserUsername:"auto.vhoteamlead1@hearings.reform.hmcts.net");

        Assert.Pass();
    }
}