namespace UI.Common.Configuration;

public class ApiClientConfiguration
{
    public string LaunchDarklyKey { get; set; }
    public string Authority { get;set; }
    public string TenantId { get;set; }
    public string ClientId { get;set; }
    public string ClientSecret { get;set; }
    public string BookingsApiUrl { get;set; }
    public string BookingsApiResourceId { get;set; }
    public string VideoApiUrl { get;set; }
    public string VideoApiResourceId { get;set; }
    public string UserApiUrl { get; set; }
    public string UserApiResourceId { get; set; }
    public string NotifyApiKey { get; set; }
}