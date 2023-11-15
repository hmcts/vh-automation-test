using System.Net.Http.Headers;
using BookingsApi.Client;
using UI.Common.Security;
using VideoApi.Client;

namespace UI.NUnitVersion;

public static class VhApiClientFactory
{
    public static async Task<VideoApiClient> CreateVideoApiClient()
    {
        var apiClientConfiguration = ConfigRootBuilder.ApiClientConfigurationInstance();
        
        var apiToken = await GenerateVideoApiToken(apiClientConfiguration);
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", apiToken);
        return VideoApiClient.GetClient(apiClientConfiguration.VideoApiUrl, httpClient);
    }
    
    public static async Task<BookingsApiClient> CreateBookingsApiClient()
    {
        var apiClientConfiguration = ConfigRootBuilder.ApiClientConfigurationInstance();
        
        var apiToken = await GenerateBookingsApiToken(apiClientConfiguration);
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", apiToken);
        return BookingsApiClient.GetClient(apiClientConfiguration.BookingsApiUrl, httpClient);
    }
    
    private static async Task<string> GenerateBookingsApiToken(ApiClientConfiguration apiClientConfiguration)
    {
        return await new TokenProvider(apiClientConfiguration)
            .GetClientAccessToken(
                apiClientConfiguration.ClientId,
                apiClientConfiguration.ClientSecret, 
                apiClientConfiguration.BookingsApiResourceId);
    }
        
    private static async Task<string> GenerateVideoApiToken(ApiClientConfiguration apiClientConfiguration)
    {
        return await new TokenProvider(apiClientConfiguration)
            .GetClientAccessToken(
                apiClientConfiguration.ClientId,
                apiClientConfiguration.ClientSecret, 
                apiClientConfiguration.VideoApiResourceId);
    }
}