namespace UI.Utilities
{
    ///<summary>
    /// Class representing application configuration
    ///</summary>
    public class SystemConfiguration
    {
        public EnvironmentConfigSettings DevelopmentEnvironmentConfigSettings { get; set; }
        public EnvironmentConfigSettings AcceptanceEnvironmentConfigSettings { get; set; }
        public EnvironmentConfigSettings ProductionEnvironmentConfigSettings { get; set; }
    }
}
