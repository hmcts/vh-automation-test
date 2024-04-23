namespace UI.AutomationTests.Admin.GetAudioLink;

public class GetVideoHearingAudioLinkTests : AdminWebUiTest
{
    [Category("Daily")]
    [Test]
    [FeatureToggleSetting(FeatureToggle.AudioSearchToggleKey, true)]
    public void GetAnAudioLinkForAVideoHearing()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var getAudioFilePage = dashboardPage.GoToGetAudioFileLink();
        var caseNumber = TestDataConfig.AudioLinkCaseNumber;
        getAudioFilePage.EnterVideoHearingCaseDetailsAndCopyLink(caseNumber);

        Assert.Pass();
    }
}