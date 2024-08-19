using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;
using LogLevel = LaunchDarkly.Logging.LogLevel;
using Logs = LaunchDarkly.Logging.Logs;

namespace UI.AutomationTests.Utilities;

public sealed class FeatureToggle
{
    public const string MultiDayBookingEnhancementsToggleKey = "multi-day-booking-enhancements";
    public const string InterpreterEnhancementsToggleKey = "interpreter-enhancements";
    
    private static FeatureToggle _instance;
    private static ILdClient _ldClient;
    private static Context _context;

    public static FeatureToggle Instance()
    {
        return _instance ??= new FeatureToggle();
    }
    
    private const string LdUser = "vh-automation-test";
    
    private FeatureToggle()
    {
        var envConfigSettings = ConfigRootBuilder.EnvConfigInstance();
        var apiConfigSettings = ConfigRootBuilder.ApiClientConfigurationInstance();
        var config = LaunchDarkly.Sdk.Server.Configuration.Builder(apiConfigSettings.LaunchDarklyKey)
            .Logging(Components.Logging(Logs.ToWriter(Console.Out)).Level(LogLevel.Warn)).Build();
        _context = Context.Builder(LdUser).Name(envConfigSettings.Environment.ToLower()).Build();
        _ldClient = new LdClient(config);
    }
    
    public bool GetBoolValueWithKey(string key)
    {
        if (!_ldClient.Initialized)
        {
            throw new InvalidOperationException("LaunchDarkly client not initialized");
        }

        return _ldClient.BoolVariation(key, _context);
    }
}