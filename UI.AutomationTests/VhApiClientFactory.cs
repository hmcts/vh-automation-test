using System.Net.Http.Headers;
using BookingsApi.Client;
using Notify.Client;
using Notify.Interfaces;
using UI.Common.Security;
using UserApi.Client;
using VideoApi.Client;

namespace UI.AutomationTests;

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
    
    public static async Task<UserApiClient> CreateUserApiClient()
    {
        var apiClientConfiguration = ConfigRootBuilder.ApiClientConfigurationInstance();
        
        var apiToken = await GenerateUserApiToken(apiClientConfiguration);
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", apiToken);
        return UserApiClient.GetClient(apiClientConfiguration.UserApiUrl, httpClient);
    }
    
    public static IAsyncNotificationClient CreateNotificationApiClient()
    {
        var apiClientConfiguration = ConfigRootBuilder.ApiClientConfigurationInstance();
        return new NotificationClient(apiClientConfiguration.NotifyApiKey);
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
            
    private static async Task<string> GenerateUserApiToken(ApiClientConfiguration apiClientConfiguration)
    {
        return await new TokenProvider(apiClientConfiguration)
            .GetClientAccessToken(
                apiClientConfiguration.ClientId,
                apiClientConfiguration.ClientSecret, 
                apiClientConfiguration.UserApiResourceId);
    }
}