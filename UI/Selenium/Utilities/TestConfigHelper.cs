using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace UI.Utilities
{
    ///<summary>
    /// Class to work with application configuration
    ///</summary>
    public static class TestConfigHelper
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static EnvironmentConfigSettings _config;

        public static EnvironmentConfigSettings GetApplicationConfiguration()
        {
            if (_config != null)
            {
                return _config;
            }
            Logger.Info("Reading Appsetitngs Json File");
            var configRoot = BuildConfigurationRoot();
            var environmentConfig = configRoot.GetSection("SystemConfiguration:EnvironmentConfigSettings").Get<EnvironmentConfigSettings>();
            
            if (environmentConfig == null)
            {
                var exception = new Exception("EnvironmentConfigSettings section missing from configuration sources");
                Logger.Error(exception);
                throw exception;
            }
            
            VerifyEnvironmentConfigValuesHaveBeenSet(environmentConfig);
            
            //set the correct ElementWait based on execution environment
            if (environmentConfig.RunOnSaucelabs)
            {
                Debug.Assert(!string.IsNullOrEmpty(environmentConfig.SauceLabsConfiguration.SauceUsername));
                Debug.Assert(!string.IsNullOrEmpty(environmentConfig.SauceLabsConfiguration.SauceUrl));
                Debug.Assert(!string.IsNullOrEmpty(environmentConfig.SauceLabsConfiguration.SauceAccessKey));
                // TODO: should have a separate property in the class instead of overwriting this one
                environmentConfig.DefaultElementWait = environmentConfig.SaucelabsElementWait;
            }

            _config = environmentConfig; 

            return _config;
        }

        private static void VerifyEnvironmentConfigValuesHaveBeenSet(EnvironmentConfigSettings environmentConfig)
        {
            foreach (var pi in environmentConfig.GetType().GetProperties())
            {
                if (pi.PropertyType != typeof(string)) continue;
                var value = (string) pi.GetValue(environmentConfig);
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException($"Expected property {pi} is empty or has not been set");
                }
            }
        }

        private static IConfigurationRoot BuildConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("e6b90eee-2685-42f6-972e-6d17e1b85a3b")
                .AddJsonFile("passwords.json",optional:true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
