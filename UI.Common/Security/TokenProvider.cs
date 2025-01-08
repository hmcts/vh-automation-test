using Microsoft.Identity.Client;
using UI.Common.Configuration;

namespace UI.Common.Security;

public class TokenProvider(ApiClientConfiguration apiClientConfiguration)
{
    public async Task<string> GetClientAccessToken(string clientId, string clientSecret, string clientResource)
    {
        var result = await GetAuthorisationResult(clientId, clientSecret, clientResource);
        return result.AccessToken;
    }

    private async Task<AuthenticationResult> GetAuthorisationResult(string clientId, string clientSecret,
        string clientResource)
    {
        var authority = $"{apiClientConfiguration.Authority}{apiClientConfiguration.TenantId}";
        var app = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(authority)
            .Build();

        var result = await app.AcquireTokenForClient(new[] { $"{clientResource}/.default" }).ExecuteAsync();

        return result;
    }
}