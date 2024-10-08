using Selenium.Axe;

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
            result.Driver.CreateAxeHtmlReport(result.Result, filename, ReportTypes.Violations);
        }
        
        // TODO merge the json into a single report
    }

    public static bool HasViolations()
    {
        return Results.Any(r =>
            r.Result.Violations.Any(x => x.Impact != "minor"));
    }
    
    private static string GenerateFilename()
    {
        var filename = $"{Guid.NewGuid()}_AccessibilityReport.html";
        var stagingDir = Environment.GetEnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY");
        if (!string.IsNullOrEmpty(stagingDir))
        {
            filename = Path.Join(stagingDir, filename);
        }

        return filename;
    }
}