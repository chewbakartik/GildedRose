using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GildedRose.Entities;
using GildedRose.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace GildedRose.Tests.Integration.Controllers
{
    public class ItemControllerTest : AbstractIntegrationTest
    {
        public ItemControllerTest(TestServerFixture testServerFixture) : base(testServerFixture) { }

        [Fact]
        public async Task Add_ShouldReturnUnauthorizedWhenNotAuthenticated()
        {
            var requestData = new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 10
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/items/add", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnForbiddenWhenNotAdmin()
        {
            // Get JWT from Auth0
            var token = Auth0Helper.GetTestAccountToken();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/items/add");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var requestData = new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 10
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnOkWhenAdmin()
        {
            // Get JWT from Auth0
            var token = Auth0Helper.GetAdminAccountToken();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/items/add");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var requestData = new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 10
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<Item>(responseContent);
            Assert.NotNull(item);
            Assert.Equal("Item1", item.Name);
            Assert.Equal("Description1", item.Description);
            Assert.Equal(9.99, item.Price);
            Assert.Equal(10, item.Quantity);
        }
    }
}
