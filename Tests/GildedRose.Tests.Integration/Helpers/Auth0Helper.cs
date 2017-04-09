using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GildedRose.Tests.Integration.Helpers
{
    public static class Auth0Helper
    {
        public static string GetTestAccountToken()
        {
            return GetToken("TestUser");
        }

        public static string GetTest2AccountToken()
        {
            return GetToken("Test2User");
        }

        public static string GetAdminAccountToken()
        {
            return GetToken("AdminUser");
        }

        public static string GetToken(string userConfigSection)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var client = new RestClient($"https://{config["Auth0:Domain"]}/oauth/ro");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-type", "application/json");
            var requestData = new
            {
                client_id = config["Auth0:ClientId"],
                connection = "Username-Password-Authentication",
                grant_type = "password",
                username = config[$"Auth0:{userConfigSection}:email"],
                password = config[$"Auth0:{userConfigSection}:password"],
                scope = "openid email roles",
                id_token = "openid"
            };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(requestData);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            var jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            return jsonResponse.GetValue("id_token").ToString();
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
