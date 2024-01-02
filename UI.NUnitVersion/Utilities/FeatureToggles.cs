using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;
using LogLevel = LaunchDarkly.Logging.LogLevel;
using Logs = LaunchDarkly.Logging.Logs;


namespace UI.NUnitVersion.Utilities;
public static class FeatureToggles
{
    private static readonly ILdClient _ldClient;
    private static readonly Context _context;
    private const string LdUser = "vh-admin-web";
    private const string BookAndConfirmToggleKey = "Book_and_Confirm";
    private const string Dom1EnabledToggleKey = "dom1";
    private const string ReferenceDataToggleKey = "reference-data";
    private const string UseV2ApiToggleKey = "use-bookings-api-v2";
    private const string EJudFeatureToggleKey = "ejud-feature";
    private const string HrsFeatureToggleKey = "hrs-integration";
    private const string AudioSearchToggleKey = "hide-audio-search-tile";
    
    static FeatureToggles()
    {
        var envConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        var apiConfigSettings = ConfigRootBuilder.ApiClientConfigurationInstance();
        var config = LaunchDarkly.Sdk.Server.Configuration.Builder(apiConfigSettings.LaunchDarklyKey)
            .Logging(Components.Logging(Logs.ToWriter(Console.Out)).Level(LogLevel.Warn)).Build();
        _context = Context.Builder(LdUser).Name(envConfigSettings.Environment.ToLower()).Build();
        _ldClient = new LdClient(config);
    }

    public static bool BookAndConfirmToggle()
    {
        return GetBoolValueWithKey(BookAndConfirmToggleKey);
    }

    public static bool Dom1Enabled()
    {
        return GetBoolValueWithKey(Dom1EnabledToggleKey);
    }
    
    public static bool ReferenceDataToggle()
    {
        return GetBoolValueWithKey(ReferenceDataToggleKey);
    }

    public static bool EJudEnabled()
    {
        return GetBoolValueWithKey(EJudFeatureToggleKey);
    }
    
    public static bool HrsEnabled()
    {
        return GetBoolValueWithKey(HrsFeatureToggleKey);
    }

    public static bool AudioSearchEnabled()
    {
        return GetBoolValueWithKey(AudioSearchToggleKey);
    }

    public static bool UseV2Api()
    {
        return GetBoolValueWithKey(UseV2ApiToggleKey);
    }
    
    private static bool GetBoolValueWithKey(string key)
    {
        if (!_ldClient.Initialized)
        {
            throw new InvalidOperationException("LaunchDarkly client not initialized");
        }

        return _ldClient.BoolVariation(key, _context);
    }

}