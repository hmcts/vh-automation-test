using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Support.Extensions;

namespace UI.AutomationTests.Reporters;

public class TestReporter
{
    private ExtentReports _extent;
    private ExtentTest _test;
    private int imageCount;

    public void SetupReport()
    {
        imageCount = 0;
        var dir = TestContext.CurrentContext.TestDirectory;
        var fileName = "UI_Test_Demo" + ".html";
        var htmlReporter = new ExtentHtmlReporter(Path.Join(dir, fileName));
        TestContext.WriteLine(Path.Join(dir, fileName));
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);
    }

    public void Flush()
    {
        _extent.Flush();
    }

    public void SetupTest(string testName)
    {
        _test = _extent.CreateTest(testName);
    }

    public void ProcessTest()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
            ? ""
            : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
        Status logstatus;

        switch (status)
        {
            case TestStatus.Failed:
                logstatus = Status.Fail;
                break;
            case TestStatus.Inconclusive:
                logstatus = Status.Warning;
                break;
            case TestStatus.Skipped:
                logstatus = Status.Skip;
                break;
            default:
                logstatus = Status.Pass;
                break;
        }

        _test.Log(logstatus, "Test ended with " + logstatus + stacktrace);

        _extent.Flush();
    }

    public void CaptureScreenshot(IWebDriver driver, string nodeName = null, string nodeDescription = null)
    {
        imageCount += 1;
        var testName = TestContext.CurrentContext.Test.Name;
        var imageFileName = $"{testName}_{imageCount}.png";
        Directory.CreateDirectory(Path.Join(TestContext.CurrentContext.TestDirectory, "images", testName));
        var screenshotFilePath =
            Path.Combine(Path.Join(TestContext.CurrentContext.TestDirectory, "images", testName, imageFileName));
        driver.TakeScreenshot().SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
        if (string.IsNullOrWhiteSpace(nodeName))
            _test.AddScreenCaptureFromPath(screenshotFilePath);
        else
            _test.CreateNode(nodeName, nodeDescription).AddScreenCaptureFromPath(screenshotFilePath);
    }
}