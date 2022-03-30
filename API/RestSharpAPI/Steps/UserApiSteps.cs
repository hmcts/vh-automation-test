using RestSharpApi.Hooks;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using NUnit.Framework;
using Bookings;
using Notification;
using User;
using Video;
using System.Collections.Generic;

namespace RestSharpApi.Steps
{
    [Binding]
    public class UserApiSteps : RestSharpHooks

    {
        private static UserApi UserApiService;

        protected static ScenarioContext _scenarioContext;

        public UserApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext=scenarioContext;
            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetServiceToServiceToken());
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

        protected string GetServiceToServiceToken()
        {
            AuthenticationResult result;
            var credential = new ClientCredential(config.clientid, config._clientSecret);
            var authContext = new AuthenticationContext($"{config._authority}{config._tenetid}");
            try
            {
                result = authContext.AcquireTokenAsync(config.bookingsapiResourceId, credential).Result;
            }
            catch (AdalException)
            {
                throw new UnauthorizedAccessException();
            }
            return result.AccessToken;
        }

    }
}

