namespace UI.NUnitVersion.Drivers;

public class LocalChromeVhDriver : IVhDriver
{
    private IWebDriver _driver;

    public LocalChromeVhDriver()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        var cService = ChromeDriverService.CreateDefaultService();
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("start-maximized");
        chromeOptions.AddArgument("no-sandbox");
        chromeOptions.AddArguments("--use-fake-ui-for-media-stream");
        chromeOptions.AddArguments("--use-fake-device-for-media-stream");
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
        throw new NotSupportedException("Need to integrate with a local reporter (i.e. ExtentReports)");
    }
}