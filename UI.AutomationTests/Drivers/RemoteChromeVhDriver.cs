using OpenQA.Selenium.Remote;
using Polly;
using Polly.Retry;

namespace UI.AutomationTests.Drivers;

public class RemoteChromeVhDriver : IVhDriver
{
    private RemoteWebDriver _driver;

    public RemoteChromeVhDriver(string platform = "Windows 11", string browserVersion = "latest",
        string username = null)
    {
        var envConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        var chromeOptions = new ChromeOptions
        {
            PlatformName = platform,
            BrowserVersion = browserVersion
        };

        chromeOptions.AddArguments("start-maximized");
        chromeOptions.AddArgument("no-sandbox");
        chromeOptions.AddArguments("--use-fake-ui-for-media-stream");
        chromeOptions.AddArguments("--use-fake-device-for-media-stream");
        
        // this is the name for a build in SauceLabs
        var buildName = Environment.GetEnvironmentVariable("TF_BUILD") == null
            ? BuildName.GetBuildNameForLocal().Trim()
            : BuildName.GetBuildNameForSauceLabs(chromeOptions.BrowserName, chromeOptions.BrowserVersion,
                chromeOptions.PlatformName, GetEnvName()).Trim();
        if (envConfigSettings.EnableAccessibilityCheck) buildName += "-Accessibility";

        var sauceLabsConfiguration = envConfigSettings.SauceLabsConfiguration;

        chromeOptions.AddAdditionalOption("username", sauceLabsConfiguration.SauceUsername);
        chromeOptions.AddAdditionalOption("accessKey", sauceLabsConfiguration.SauceAccessKey);

        // this is the name for a test in a build in SauceLabs. Giving unique names to tests allows us to see them in SauceLabs
        var testName = TestContext.CurrentContext.Test.Name;
        if (envConfigSettings.EnableAccessibilityCheck) testName += "-Accessibility";
        if (!string.IsNullOrWhiteSpace(username)) testName += $"-{username}";

        var sauceOptions = new Dictionary<string, object>
        {
            { "build", buildName },
            { "name", testName },
            { "timeZone", "London" },
            { "maxDuration", sauceLabsConfiguration.MaxDurationInSeconds },
            { "commandTimeout", sauceLabsConfiguration.CommandTimeoutInSeconds },
            { "idleTimeout", sauceLabsConfiguration.IdleTimeoutInSeconds },
            { "screenResolution", sauceLabsConfiguration.WindowsScreenResolution },
            { "username", sauceLabsConfiguration.SauceUsername },
            { "accessKey", sauceLabsConfiguration.SauceAccessKey },
            { "extendedDebugging", true }
        };
        chromeOptions.AddAdditionalOption("sauce:options", sauceOptions);
        var remoteUrl = new Uri(sauceLabsConfiguration.SecureSauceUrl);
        var commandTimeout = TimeSpan.FromSeconds(sauceLabsConfiguration.CommandTimeoutInSeconds);
        
        // Retry policy with Polly
        AsyncRetryPolicy retryPolicy = Policy
            .Handle<WebDriverException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        retryPolicy.ExecuteAsync(() =>
        {
            var remoteDriver = new RemoteWebDriver(remoteUrl, chromeOptions.ToCapabilities(), commandTimeout);
            _ =remoteDriver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(30));
            remoteDriver.FileDetector = new LocalFileDetector();
            _driver = remoteDriver;
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }

    private string GetEnvName()
    {
        var apiClientConfiguration = ConfigRootBuilder.ApiClientConfigurationInstance();
        if (apiClientConfiguration.BookingsApiResourceId.Contains(".dev."))
        {
            return "Dev";
        }
        
        if (apiClientConfiguration.BookingsApiResourceId.Contains(".test."))
        {
            return "Test";
        }

        if (apiClientConfiguration.BookingsApiResourceId.Contains(".staging."))
        {
            return "Staging";
        }
        
        if (apiClientConfiguration.BookingsApiResourceId.Contains(".demo."))
        {
            return "Demo";
        }

        return null;
    }
    
    public IWebDriver GetDriver()
    {
        if (_driver == null) throw new NullReferenceException("Driver has not been initialised");
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
            TestContext.Out.WriteLine($"<{e.GetType().Name}> Failed to report test status to SauceLabs: {e.Message}");
        }
    }
}