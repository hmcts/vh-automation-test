namespace UI.Common.Configuration;

public class SystemConfigSettings
{
    public string TestResultsDirectory { get; set; }
    public string ReportLocation { get; set; }
    public string ImageLocation { get; set; }
    public bool RunOnSauceLabs { get; set; }
    public bool RunHeadlessBrowser { get; set; }
    public bool EnableAccessibilityCheck { get; set; }
    public string AccessibilityReportFilePath { get; set; }
    public string AccessibilityHtmlReportFilePath { get; set; }
    public SauceLabsConfiguration SauceLabsConfiguration { get; set; }
}