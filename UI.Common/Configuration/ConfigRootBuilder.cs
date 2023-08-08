using Microsoft.Extensions.Configuration;
using UI.NUnitVersion.Configuration;

namespace UI.Common.Configuration;

public static class ConfigRootBuilder
{
    private const string UserSecretId = "e6b90eee-2685-42f6-972e-6d17e1b85a3b";

    private static IConfigurationRoot Build(string userSecretId = UserSecretId, bool useSecrets = true)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", true)
            .AddJsonFile("appsettings.Production.json", true); // CI write variables in the pipeline to this file

        if (useSecrets) builder = builder.AddUserSecrets(userSecretId);

        return builder.AddEnvironmentVariables()
            .Build();
    }

    private static IConfigurationRoot? _instance;
    private static EnvironmentConfigSettings? _envConfigInstance;
    private static ApiClientConfiguration? _apiClientConfigInstance;
    private static TestDataConfiguration? _testDataConfigurationInstance;

    private static IConfigurationRoot Instance(string userSecretId = UserSecretId, bool useSecrets = true) =>
        _instance ??= Build(userSecretId, useSecrets);
    
    public static EnvironmentConfigSettings EnvConfigInstance(string userSecretId = UserSecretId, bool useSecrets = true){
        if (_envConfigInstance != null) return _envConfigInstance;
        _envConfigInstance = new EnvironmentConfigSettings();
        Instance(userSecretId, useSecrets).GetSection("SystemConfiguration:EnvironmentConfigSettings").Bind(_envConfigInstance);
        return _envConfigInstance;
    }

    public static ApiClientConfiguration ApiClientConfigurationInstance(string userSecretId = UserSecretId,
        bool useSecrets = true)
    {
        if (_apiClientConfigInstance != null) return _apiClientConfigInstance;
        _apiClientConfigInstance = new ApiClientConfiguration();
        Instance(userSecretId, useSecrets).GetSection("ApiClientConfiguration").Bind(_apiClientConfigInstance);
        return _apiClientConfigInstance;
    }
    
    public static TestDataConfiguration TestDataConfigurationInstance(string userSecretId = UserSecretId,
        bool useSecrets = true)
    {
        if (_testDataConfigurationInstance != null) return _testDataConfigurationInstance;
        _testDataConfigurationInstance = new TestDataConfiguration();
        Instance(userSecretId, useSecrets).GetSection("TestDataConfiguration").Bind(_testDataConfigurationInstance);
        return _testDataConfigurationInstance;
    }
}