namespace UI.Common.Configuration;

public class SauceLabsConfiguration
{
    public string MacScreenResolution => "2360x1770";
    public string WindowsScreenResolution => "2560x1600";
    public string SauceUsername { get; set; }
    public string SauceAccessKey { get; set; }
    public string SauceUrl { get; set; }
    public string SecureSauceUrl { get; set; }

    // SauceLabOptions
    public int MaxDurationInSeconds => 60 * 10;
    public int IdleTimeoutInSeconds => 60 * 7;
    public int CommandTimeoutInSeconds => 120;
}