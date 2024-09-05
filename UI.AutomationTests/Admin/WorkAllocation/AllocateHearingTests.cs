namespace UI.AutomationTests.Admin.WorkAllocation;


public class AllocateHearingTests : AdminWebUiTest
{
    private HearingDetailsResponseV2 _hearing;
    
    [Test]
    public async Task AllocateAJusticeUserToAHearing()
    {
        var teamMemberUsername = WorkAllocationTestData.JusticeUserUsername;
        var justiceUser = await CreateVhTeamLeaderJusticeUserIfNotExist(teamMemberUsername);
        
        await BookBasicHearing();
        var driver = VhDriver.GetDriver();
        await driver.Navigate().GoToUrlAsync(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var manageWorkAllocationPage = dashboardPage.GoToManageWorkAllocation();
        manageWorkAllocationPage.AllocateJusticeUserToHearing(
            caseNumber: _hearing.Cases[0].Number,
            justiceUserDisplayName: justiceUser.FullName,
            justiceUserUsername: justiceUser.Username);

        Assert.Pass();
    }
    
    private async Task BookBasicHearing()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var request = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: date);
        _hearing = await BookingsApiClient.BookNewHearingWithCodeAsync(request);
        TestHearingIds.Add(_hearing.Id.ToString());
    }
}