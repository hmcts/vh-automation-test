namespace UI.PageModels.Utilities;

public static class AccessibilityResultCollection
{
    private static List<AccessibilityResult> Results { get; } = [];

    public static bool Add(AccessibilityResult result)
    {
        var alreadyExists = Results.Find(r => r.Result.Url == result.Result.Url) != null;
        if (alreadyExists) return false;
        
        Results.Add(result);

        return true;
    }
    
    public static void Clear()
    {
        Results.Clear();
    }

    public static bool HasViolations()
    {
        return Results.Any(r =>
            r.Result.Violations.Any(x => x.Impact != "minor"));
    }
}