namespace UI.Common.Configuration;

public class SystemConfigSettings
{
    public string TestResultsDirectory { get; set; }
    public string ReportLocation { get; set; }
    public string ImageLocation { get; set; }
    public bool RunOnSaucelabs { get; set; }
    public bool EnableAccessibilityCheck { get; set; }
    public string? AccessibilityReportFilePath { get; set; }
    public int PipelineElementWait { get; set; }
    public int SaucelabsElementWait { get; set; }
    public SauceLabsConfiguration SauceLabsConfiguration { get; set; }
}