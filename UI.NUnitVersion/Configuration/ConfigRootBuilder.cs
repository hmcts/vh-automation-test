namespace UI.NUnitVersion.Configuration;

public static class ConfigRootBuilder
{
    private const string UserSecretId = "e6b90eee-2685-42f6-972e-6d17e1b85a3b";
    public static IConfigurationRoot Build(string userSecretId = UserSecretId, bool useSecrets = true)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", true)
            .AddJsonFile("appsettings.Production.json", true); // CI write variables in the pipeline to this file

        if (useSecrets)
        {
            builder = builder.AddUserSecrets(userSecretId);
        }

        return builder.AddEnvironmentVariables()
            .Build();
    }
}
