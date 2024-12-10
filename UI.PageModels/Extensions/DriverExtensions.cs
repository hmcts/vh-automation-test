namespace UI.PageModels.Extensions;

public static class DriverExtensions
{
    private static readonly Dictionary<IWebDriver, string> DriverUsernames = new();
    
    public static void StoreUsername(this IWebDriver driver, string username)
    {
        DriverUsernames[driver] = username;
    }
    
    public static string GetStoredUsername(this IWebDriver driver)
    {
        return DriverUsernames[driver];
    }
}