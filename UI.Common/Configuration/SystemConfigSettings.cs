namespace UI.Common.Configuration;

public class SystemConfigSettings
{
    public string ReportLocation { get; set; }
    public bool RunOnSauceLabs { get; set; }
    public bool RunHeadlessBrowser { get; set; }
    public bool EnableAccessibilityCheck { get; set; }
    public SauceLabsConfiguration SauceLabsConfiguration { get; set; }
}