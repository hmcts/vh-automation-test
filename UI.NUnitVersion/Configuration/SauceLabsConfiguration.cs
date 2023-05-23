namespace UI.NUnitVersion.Configuration;

public class SauceLabsConfiguration
{
    public string PlatformName { get; set; }
    public string BrowserName { get; set; }
    public string DeviceName { get; set; }
    public string PlatformVersion { get; set; }
    public string AppiumVersion { get; set; }
    public string SauceUsername { get; set; }
    public string SauceAccessKey { get; set; }
    public string SauceUrl { get; set; }
    public string SecureSauceUrl { get; set; }
    public string Orientation { get; set; }
    
    // SauceLabOptions
    public int MaxDurationInSeconds { get; set; } = 60 * 10;
    public int IdleTimeoutInSeconds { get; set; } = 60 * 7;
    public int CommandTimeoutInSeconds { get; set; } = 60 * 3;
    public string WindowsScreenResolution = "2560x1600";
    public string MacScreenResolution = "2360x1770";
}