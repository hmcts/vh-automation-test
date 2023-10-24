namespace UI.Common.Configuration;

public class ApiClientConfiguration
{
    public string Authority { get;set; }
    public string TenantId { get;set; }
    public string ClientId { get;set; }
    public string ClientSecret { get;set; }
    public ApiClientDetails BookingsApi { get;set; }
    public ApiClientDetails VideoApi { get;set; }
}

public class ApiClientDetails
{
    public string Url { get;set; }
    public string ResourceId { get;set; }
}