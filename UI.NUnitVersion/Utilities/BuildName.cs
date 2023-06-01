namespace UI.NUnitVersion.Utilities;

public class BuildName
{
    private static string _buildName;
    public static string GetBuildNameForSauceLabs(string browserName, string browserVersion, string platformName)
    {
        if (!string.IsNullOrWhiteSpace(_buildName)) return _buildName;
        // move this into a static file so only made once per run
        var attemptNumber = GetAttemptNumber();
        _buildName = $"{GetBuildDefinition()}{GetGitVersionNumber()} {DateTime.Now:dd-mm-yy-hh-mm}     [ {browserName} | {platformName} | {browserVersion} ] {attemptNumber}";
        return _buildName;
    }
    
    private static string GetAttemptNumber()
    {
        var attemptNumber = Environment.GetEnvironmentVariable("Build_AttemptNumber");
        if (string.IsNullOrWhiteSpace(attemptNumber)) return string.Empty;
        return Convert.ToInt32(attemptNumber) > 1 ? $" : Attempt {attemptNumber}" : string.Empty;
    }
        
    private static string GetGitVersionNumber()
    {
        var gitVersionNumber = Environment.GetEnvironmentVariable("GITVERSION_FULLSEMVER");
        return !string.IsNullOrEmpty(gitVersionNumber) ? $" | {gitVersionNumber}" : string.Empty;
    }
        
    private static string GetBuildDefinition()
    {
        var definition = Environment.GetEnvironmentVariable("BUILD_DEFINITIONNAME")?
                             .ToLower()
                             .Replace("hmcts.vh-", "")
                             .Replace("-", " ")
                             .Replace("cd", "")
                             .Replace("webnightly", " Web Nightly")
                             .Replace("web", " Web")
                         ?? string.Empty;
        return new CultureInfo("en-GB", false).TextInfo.ToTitleCase(definition);
    }
}