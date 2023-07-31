using System.Net.Http.Headers;
using BookingsApi.Client;
using UI.Common.Security;

namespace UI.NUnitVersion;

public static class VhApiClientFactory
{
    public static async Task<BookingsApiClient> CreateBookingsApiClient()
    {
        var apiClientConfiguration = ConfigRootBuilder.ApiClientConfigurationInstance();
        
        var apiToken = await GenerateBookingsApiToken(apiClientConfiguration);
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", apiToken);
        return BookingsApiClient.GetClient(apiClientConfiguration.BookingsApi.Url, httpClient);
    }
    
    private static async Task<string> GenerateBookingsApiToken(ApiClientConfiguration apiClientConfiguration)
    {
        return await new TokenProvider(apiClientConfiguration).GetClientAccessToken(apiClientConfiguration.ClientId,
            apiClientConfiguration.ClientSecret, apiClientConfiguration.BookingsApi.ResourceId);
    }
}