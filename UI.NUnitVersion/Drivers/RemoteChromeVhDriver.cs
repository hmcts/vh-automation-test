using OpenQA.Selenium.Remote;
using UI.NUnitVersion.Utilities;

namespace UI.NUnitVersion.Drivers;

public class RemoteChromeVhDriver : IVhDriver
{
    private RemoteWebDriver _driver;

    public RemoteChromeVhDriver(string platform = "Windows 11", string browserVersion = "latest")
    {
        var envConfigSettings = ConfigRootBuilder.Build().GetSection("SystemConfiguration:EnvironmentConfigSettings")
            .Get<EnvironmentConfigSettings>();
        var driverOptions = new ChromeOptions
        {
            PlatformName = platform,
            BrowserVersion = browserVersion
        };

        var buildName = Environment.GetEnvironmentVariable("TF_BUILD") == null ? 
            $"local-{Environment.MachineName}-{Environment.UserName}-{DateTime.Now:dd-mm-yy-hh-mm}" : 
            BuildName.GetBuildNameForSauceLabs(driverOptions.BrowserName, driverOptions.BrowserVersion, driverOptions.PlatformName);

        var sauceLabsConfiguration = envConfigSettings.SauceLabsConfiguration;
        
        driverOptions.AddAdditionalOption("username", sauceLabsConfiguration.SauceUsername);
        driverOptions.AddAdditionalOption("accessKey", sauceLabsConfiguration.SauceAccessKey);
        
        var sauceOptions = new Dictionary<string, object>
        {
            {"build", buildName},
            {"name", TestContext.CurrentContext.Test.Name},
            {"timeZone", "London"},
            {"maxDuration", sauceLabsConfiguration.MaxDurationInSeconds},
            {"commandTimeout", sauceLabsConfiguration.CommandTimeoutInSeconds},
            {"idleTimeout", sauceLabsConfiguration.IdleTimeoutInSeconds},
            {"screenResolution", sauceLabsConfiguration.WindowsScreenResolution},
        };
        driverOptions.AddAdditionalOption("sauce:options", sauceOptions);
        
        var remoteUrl = new Uri($"https://{sauceLabsConfiguration.SauceUsername}:{sauceLabsConfiguration.SauceAccessKey}@{sauceLabsConfiguration.SecureSauceUrl}");
        var remoteDriver = new RemoteWebDriver(remoteUrl, driverOptions);
        remoteDriver.FileDetector = new LocalFileDetector();
        _driver = remoteDriver;
    }

    public void CaptureScreenShot()
    {
        throw new NotImplementedException();
    }

    public IWebDriver GetDriver()
    {
        if (_driver == null)
        {
            throw new NullReferenceException("Driver has not been initialised");
        }
        return _driver;
    }

    public void Terminate()
    {
        _driver?.Quit();
        _driver = null;
    }

    public void PublishTestResult(bool passed)
    {
        try
        {
            var script = "sauce:job-result=" + (passed ? "passed" : "failed");
            _driver.ExecuteScript(script);
        }
        catch (Exception e)
        {
            TestContext.WriteLine($"<{e.GetType().Name}> Failed to report test status to SauceLabs: {e.Message}");
        }
    }
    
    
}