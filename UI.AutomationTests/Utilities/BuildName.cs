namespace UI.AutomationTests.Utilities;

public static class BuildName
{
    private static string _buildNameSauceLabs;
    private static string _buildNameLocal;

    public static string GetBuildNameForSauceLabs(string browserName, string browserVersion, string platformName, string environmentName = "Dev")
    {
        if (!string.IsNullOrWhiteSpace(_buildNameSauceLabs)) return _buildNameSauceLabs;
        var attemptNumber = GetAttemptNumber();
        _buildNameSauceLabs =
            $"{GetBuildDefinition()}{GetGitVersionNumber()} {DateTime.Now:dd-mm-yy-hh-mm}  [ {environmentName} | {browserName} | {platformName} | {browserVersion} ] {attemptNumber}";
        return _buildNameSauceLabs;
    }

    public static string GetBuildNameForLocal()
    {
        if (!string.IsNullOrWhiteSpace(_buildNameLocal)) return _buildNameLocal;
        _buildNameLocal = $"local-{Environment.MachineName}-{Environment.UserName}-{DateTime.Now:dd-mm-yy-hh-mm}";
        return _buildNameLocal;
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