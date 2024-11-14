using Microsoft.Identity.Client;
using UI.Common.Configuration;

namespace UI.Common.Security;

public class TokenProvider
{
    private readonly ApiClientConfiguration _apiClientConfiguration;

    public TokenProvider(ApiClientConfiguration apiClientConfiguration)
    {
        _apiClientConfiguration = apiClientConfiguration;
    }

    public async Task<string> GetClientAccessToken(string clientId, string clientSecret, string clientResource)
    {
        var result = await GetAuthorisationResult(clientId, clientSecret, clientResource);
        return result.AccessToken;
    }

    private async Task<AuthenticationResult> GetAuthorisationResult(string clientId, string clientSecret, string clientResource)
    {
        AuthenticationResult result;
        var authority = $"{_apiClientConfiguration.Authority}{_apiClientConfiguration.TenantId}";
        var app = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(authority)
            .Build();
            
        try
        {
            result = await app.AcquireTokenForClient(new[] {$"{clientResource}/.default"}).ExecuteAsync();
        }
        catch (MsalServiceException e)
        {
            throw new UnauthorizedAccessException();
        }

        return result;
    }
}