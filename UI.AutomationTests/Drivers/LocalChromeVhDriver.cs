using ChromeForTesting;
using WebDriverManager;

namespace UI.AutomationTests.Drivers;

public class LocalChromeVhDriver : IVhDriver
{
    private IWebDriver _driver;

    public LocalChromeVhDriver()
    {
        var chromePath = ChromeForTestingInstance.ChromePath;
        new DriverManager().SetUpDriver(new ChromeConfig());
        var cService = ChromeDriverService.CreateDefaultService();
        TestContext.Out.WriteLine($"Using chrome binary at {chromePath}");
        var chromeOptions = new ChromeOptions()
        {
            BinaryLocation = chromePath
        };
        chromeOptions.AddArguments("start-maximized");
        chromeOptions.AddArgument("no-sandbox");
        chromeOptions.AddArguments("--use-fake-ui-for-media-stream");
        chromeOptions.AddArguments("--use-fake-device-for-media-stream");

        if (Environment.GetEnvironmentVariable("TF_BUILD") != null)
        {
            chromeOptions.AddArgument("--disable-dev-shm-usage"); // Overcome limited resource problems
            chromeOptions.AddArgument("--headless"); // Run in headless mode if needed
            chromeOptions.AddArgument("--disable-gpu"); // Applicable to Windows OS only
            chromeOptions.AddArgument("--remote-debugging-port=9222"); // Debugging port
        }
        _driver = new ChromeDriver(cService, chromeOptions);
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