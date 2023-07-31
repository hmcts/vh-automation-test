using RestSharpApi.Hooks;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using Microsoft.Identity.Client;
using Bookings;
using User;

namespace RestSharpApi.Steps
{
    [Binding]
    public class UserApiSteps : RestSharpHooks

    {
        private static UserApi UserApiService;

        UserApiSteps()
        {
            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetServiceToServiceToken().Result);
            UserApiService = new UserApi(_client);
            UserApiService.BaseUrl = config.usersapi;
        }

        [Given(@"I have a userApi")]
        public async Task GivenIHaveAUserApi()
        {
            var _baseUrl = config.usersapi;
            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer","");
            var UserApiService = new BookingsApi(_client);
            UserApiService.BaseUrl = _baseUrl;
            var health1 = await UserApiService.CheckServiceHealthAsync();
            var drop = health1.App_version;
            _logger.Info("test");
            _logger.Info($"Drop is {drop}");
        }

        protected async Task<string> GetServiceToServiceToken()
        {
            AuthenticationResult result;
            var authority = $"{config._authority}{config._tenetid}";
            var app =ConfidentialClientApplicationBuilder.Create(config.clientid).WithClientSecret(config._clientSecret)
                .WithAuthority(authority).Build();
            
            try
            {
                result = await app.AcquireTokenForClient(new[] {$"{config.bookingsapiResourceId}/.default"}).ExecuteAsync();
            }
            catch (MsalServiceException)
            {
                throw new UnauthorizedAccessException();
            }

            return result.AccessToken;
        }

    }
}

