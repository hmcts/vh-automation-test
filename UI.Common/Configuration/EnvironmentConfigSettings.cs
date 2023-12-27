namespace UI.Common.Configuration;

/// <summary>
///     Class to hold Environment related settings
/// </summary>
public class EnvironmentConfigSettings : SystemConfigSettings
{
    public string Environment { get; set; }
    public string UserPassword { get; set; }
    public int DefaultElementWait { get; set; }
    public string OneMinuteElementWait { get; set; }
    public string VideoUrl { get; set; }
    public string AdminUrl { get; set; }
    public string UKConferencePhoneNumber { get; set; }
    public string LaunchDarklyKey { get; set; }

    // public string ApiUrl { get; set; }
    // public string SoapApiUrl { get; set; }
    // public string ServiceUrl { get; set; }
    // public string ConnectionString { get; set; }
    // public string UserURL { get; set; }
}