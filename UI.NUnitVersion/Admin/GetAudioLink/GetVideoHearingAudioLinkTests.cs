namespace UI.NUnitVersion.Admin.GetAudioLink;

public class GetVideoHearingAudioLinkTests : AdminWebUiTest
{
    [Test]
    public void GetAnAudioLinkForAVideoHearing()
    {
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var getAudioFilePage = dashboardPage.GoToGetAudioFileLink();
        var caseNumber = "VIH 9831";
        getAudioFilePage.EnterVideoHearingCaseDetailsAndCopyLink(caseNumber);
        
        Assert.Pass();
    }
}