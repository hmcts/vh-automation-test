namespace UI.Utilities;

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
    public string Orientation { get; set; }
    public bool ConsoleLogging { get; set; }
}