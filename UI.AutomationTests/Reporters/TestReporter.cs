using System.Text;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using UI.PageModels.Extensions;
using UI.PageModels.Utilities;

namespace UI.AutomationTests.Reporters;

public static class TestReporterInstance
{
    private static TestReporter _instance;

    public static TestReporter Instance()
    {
        if (_instance != null) return _instance;
        
        _instance = new TestReporter();
        _instance.SetupReport();
        return _instance;
    }
}

public class TestReporter
{
    private ExtentReports _extent;
    private readonly Dictionary<string, ExtentTest> _tests = new();
    
    public void SetupReport()
    {
        var dir = string.IsNullOrWhiteSpace(ConfigRootBuilder.EnvConfigInstance().ReportLocation)
            ? $"{TestContext.CurrentContext.TestDirectory}/Reports"
            : ConfigRootBuilder.EnvConfigInstance().ReportLocation;
        var environment = ConfigRootBuilder.EnvConfigInstance().Environment;
        var category = GetTestCategory();

        var sb = new StringBuilder($"VH_UI_Test_Report_{environment}");
        if(category != String.Empty)
            sb.Append($"_{category}");
        sb.Append(".html");

        var fileName = sb.ToString();
        var htmlReporter = new ExtentSparkReporter(Path.Join(dir, fileName));
        TestContext.Out.WriteLine(Path.Join(dir, fileName));
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);
    }
    
    private static string GetTestCategory()
    {
        var categories = TestContext.CurrentContext.Test.Properties["Category"].ToList();
        return categories.Count > 0 ? categories[0].ToString() : string.Empty;
    }

    public void SetupTest(string testName, string description, string[] categories)
    {
        var test = _extent.CreateTest(testName, description);
        test.AssignCategory(categories);
        _tests.Add(testName, test);
    }

    public void ProcessTest(IWebDriver driver)
    {
        var logStatus = ConvertNUnitTestResultToExtentReportLogStatus();
        var test = _tests[TestContext.CurrentContext.Test.Name];
        var sb = new StringBuilder(TestContext.CurrentContext.Result.Message);
        sb.AppendLine(TestContext.CurrentContext.Result.StackTrace);
        LogEntry(test, logStatus, sb.ToString(), driver.TakeScreenshotAsBase64());
        _extent.Flush();
    }

    public void AddScreenshotsToReport(IWebDriver driver)
    {
        driver.TakeScreenshotAndSave("End of Test", "End of Test", ConvertNUnitTestResultToExtentReportLogStatus());
        var testName = Environment.GetEnvironmentVariable(VhPage.VHTestNameKey)!;
        var images = ScreenshotCollector.Instance().GetImages(testName);
        
        var testReport = _tests[testName];
        ExtentTest node = null;
        foreach (var screenshotDto in images)
        {
            if (node == null || node.Model.Name != screenshotDto.Page)
            {
                node = testReport.CreateNode(screenshotDto.Page);
            }
            
            LogEntry(node, screenshotDto.Status, screenshotDto.Action, screenshotDto.ImageBase64Encoded);
        }
    }
    
    private void LogEntry(ExtentTest test, Status logStatus, string message, string imageBase64Encoded)
    {
        if(logStatus == Status.Fail)
            test.Fail(new Exception(message), MediaEntityBuilder.CreateScreenCaptureFromBase64String(imageBase64Encoded).Build());
        else
            test.Log(logStatus, message, MediaEntityBuilder.CreateScreenCaptureFromBase64String(imageBase64Encoded).Build());
    }
    
    public void Flush()
    {
        _extent.Flush();
    }
    
    private static Status ConvertNUnitTestResultToExtentReportLogStatus()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
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

        return logstatus;
    }
}