using Selenium.Axe;
using UI.PageModels.Pages;

namespace UI.PageModels.Utilities;

public static class AccessibilityReport
{
    public static void Create(AccessibilityResult result, IWebDriver driver)
    {
        var filename = GenerateFilename();
        CheckAndCreateDirectory(filename);
        driver.CreateAxeHtmlReport(result.Result, filename, ReportTypes.Violations);
    }
    
    private static string GenerateFilename()
    {
        var directory = Environment.GetEnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY") ?? string.Empty;
        var testName = Environment.GetEnvironmentVariable(VhPage.VHTestNameKey);
        directory = !string.IsNullOrEmpty(directory) ? Path.Join(directory, testName) : testName;

        if (!string.IsNullOrEmpty(directory) && !directory.EndsWith('/'))
            directory += "/";
        
        var filename = $"{directory}{Guid.NewGuid()}_AccessibilityReport.html";

        return filename;
    }

    private static void CheckAndCreateDirectory(string filename)
    {
        var directory = Path.GetDirectoryName(filename) ?? string.Empty;
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}