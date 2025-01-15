using WebDriverManager;

namespace UI.AutomationTests.Drivers;

public class LocalChromeVhDriver : IVhDriver
{
    private IWebDriver _driver;

    public LocalChromeVhDriver(string videoFileName = null)
    {
        // download the latest chrome
        new DriverManager().SetUpDriver(new ChromeConfig());
        var chromeOptions = new ChromeOptions();
        
        chromeOptions.AddArgument("--lang=en-GB"); // Set the region to English (UK)
        chromeOptions.AddArgument("--start-maximized");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--mute-audio");
        chromeOptions.AddArgument("--use-fake-ui-for-media-stream");
        chromeOptions.AddArgument("--use-fake-device-for-media-stream");
        chromeOptions.AddArgument("--use-fake-ui-for-media-stream");

        if (videoFileName != null)
        {
            var videoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockVideos", videoFileName);
            if (!File.Exists(videoFilePath))
            {
                throw new FileNotFoundException($"Video file not found: {videoFilePath}");
            }
            chromeOptions.AddArgument($"--use-file-for-fake-video-capture={videoFilePath}");
        }

        var envConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        if (envConfigSettings.RunHeadlessBrowser)
        {
            chromeOptions.AddArgument("--disable-dev-shm-usage"); // Overcome limited resource problems
            chromeOptions.AddArgument("--headless"); // Run in headless mode if needed
        }
        _driver = new ChromeDriver(chromeOptions);
        var lang = (string)((IJavaScriptExecutor)_driver).ExecuteScript("return navigator.language || navigator.userLanguage");
        TestContext.Out.WriteLine($"Browser language: {lang}");

    }

    public IWebDriver GetDriver()
    {
        return _driver;
    }

    public void Terminate()
    {
        _driver?.Quit();
        _driver = null;
    }

    public void PublishTestResult(bool passed)
    {
        // throw new NotSupportedException("Need to integrate with a local reporter (i.e. ExtentReports)");
    }
}