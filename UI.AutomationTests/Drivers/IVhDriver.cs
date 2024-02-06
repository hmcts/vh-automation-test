namespace UI.AutomationTests.Drivers;

public interface IVhDriver
{
    IWebDriver GetDriver();
    void Terminate();
    void PublishTestResult(bool passed);
}