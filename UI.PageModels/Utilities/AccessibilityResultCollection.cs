using Selenium.Axe;
using UI.PageModels.Pages;

namespace UI.PageModels.Utilities;

public static class AccessibilityResultCollection
{
    private static List<AccessibilityResult> Results { get; } = [];
    
    public static void Add(AccessibilityResult result)
    {
        var alreadyExists = Results.Find(r => r.Result.Url == result.Result.Url) != null;
        if (alreadyExists) return;
        
        Results.Add(result);
    }

    public static void Clear()
    {
        Results.Clear();
    }

    public static void CreateReports()
    {
        foreach (var result in Results)
        {
            var filename = GenerateFilename();
            CheckAndCreateDirectory(filename);
            result.Driver.CreateAxeHtmlReport(result.Result, filename, ReportTypes.Violations);
        }
    }

    public static bool HasViolations()
    {
        return Results.Any(r =>
            r.Result.Violations.Any(x => x.Impact != "minor"));
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