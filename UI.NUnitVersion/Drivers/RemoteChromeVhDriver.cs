using System.Globalization;
using OpenQA.Selenium.Remote;

namespace UI.NUnitVersion.Drivers;

public class RemoteChromeVhDriver : IVhDriver
{
    private RemoteWebDriver _driver;
    private readonly ChromeOptions _driverOptions;

    public RemoteChromeVhDriver(string platform = "Windows 11", string browserVersion = "latest")
    {
        var envConfigSettings = ConfigRootBuilder.Build().GetSection("SystemConfiguration:EnvironmentConfigSettings")
            .Get<EnvironmentConfigSettings>();
        _driverOptions = new ChromeOptions
        {
            PlatformName = platform,
            BrowserVersion = browserVersion
        };

        var buildName = Environment.GetEnvironmentVariable("TF_BUILD") == null ? 
            $"local-{Environment.MachineName}-{Environment.UserName}-{DateTime.Now:dd-mm-yy-hh-mm}" : 
            GetBuildNameForSauceLabs();

        var sauceLabsConfiguration = envConfigSettings.SauceLabsConfiguration;
        
        _driverOptions.AddAdditionalOption("username", sauceLabsConfiguration.SauceUsername);
        _driverOptions.AddAdditionalOption("accessKey", sauceLabsConfiguration.SauceAccessKey);
        
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
        _driverOptions.AddAdditionalOption("sauce:options", sauceOptions);
        
        var remoteUrl = new Uri($"https://{sauceLabsConfiguration.SauceUsername}:{sauceLabsConfiguration.SauceAccessKey}@{sauceLabsConfiguration.SecureSauceUrl}");
        var remoteDriver = new RemoteWebDriver(remoteUrl, _driverOptions);
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
    
    private string GetBuildNameForSauceLabs()
    {
        var attemptNumber = GetAttemptNumber();
        var build = $"{GetBuildDefinition()}{GetGitVersionNumber()} {DateTime.Now:dd-mm-yy-hh-mm}     [ {_driverOptions.BrowserName} | {_driverOptions.PlatformName} | {_driverOptions.BrowserVersion} ] {attemptNumber}";
        return build;
    }
    
    private static string GetAttemptNumber()
    {
        var attemptNumber = Environment.GetEnvironmentVariable("Build_AttemptNumber");
        if (string.IsNullOrWhiteSpace(attemptNumber)) return string.Empty;
        return Convert.ToInt32(attemptNumber) > 1 ? $" : Attempt {attemptNumber}" : string.Empty;
    }
        
    private static string GetGitVersionNumber()
    {
        var gitVersionNumber = Environment.GetEnvironmentVariable("GITVERSION_FULLSEMVER");
        return !string.IsNullOrEmpty(gitVersionNumber) ? $" | {gitVersionNumber}" : string.Empty;
    }
        
    private static string GetBuildDefinition()
    {
        var definition = Environment.GetEnvironmentVariable("BUILD_DEFINITIONNAME")?
                             .ToLower()
                             .Replace("hmcts.vh-", "")
                             .Replace("-", " ")
                             .Replace("cd", "")
                             .Replace("webnightly", " Web Nightly")
                             .Replace("web", " Web")
                         ?? string.Empty;
        return new CultureInfo("en-GB", false).TextInfo.ToTitleCase(definition);
    }
}