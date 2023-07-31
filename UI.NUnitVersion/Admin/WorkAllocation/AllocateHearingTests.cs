namespace UI.NUnitVersion.Admin.WorkAllocation;

[Category("Daily")]
public class AllocateHearingTests : AdminWebUiTest
{
    private HearingDetailsResponse _hearing;
    
    [Test]
    public async Task AllocateAJusticeUserToAHearing()
    {
        await BookBasicHearing();
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var manageWorkAllocationPage = dashboardPage.GoToManageWorkAllocation();
        manageWorkAllocationPage.AllocateJusticeUserToHearing(
            caseNumber: _hearing.Cases[0].Number,
            justiceUserDisplayName: "Auto VHoteamleader1",
            justiceUserUsername: "auto.vhoteamlead1@hearings.reform.hmcts.net");

        Assert.Pass();
    }
    
    private async Task BookBasicHearing()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var request = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: date);
        _hearing = await BookingsApiClient.BookNewHearingAsync(request);
    }
    
    protected override async Task CleanUp()
    {
        if (_hearing != null)
        {
            TestContext.WriteLine($"Removing Hearing {_hearing.Id}");
            await BookingsApiClient.RemoveHearingAsync(_hearing.Id);
            _hearing = null;
        }
    }
}