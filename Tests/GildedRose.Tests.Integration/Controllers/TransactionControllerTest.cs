using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GildedRose.Data;
using GildedRose.Data.Repositories;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;
using GildedRose.Tests.Integration.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace GildedRose.Tests.Integration.Controllers
{
    public class TransactionControllerTest : AbstractIntegrationTest
    {
        public string AdminToken { get; set; }
        public string TestToken { get; set; }
        public string Test2Token { get; set; }

        public TransactionControllerTest(TestServerFixture testServerFixture) : base(testServerFixture)
        {
            AdminToken = Auth0Helper.GetAdminAccountToken();
            TestToken = Auth0Helper.GetTestAccountToken();
            Test2Token = Auth0Helper.GetTest2AccountToken();
        }

        [Fact]
        public async Task Get_ShouldReturnNotFoundWhenNotAuthenticated()
        {
            var transaction = await CreateData();
            var response = await client.GetAsync($"api/transactions/get/{transaction.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWhenUserIsAdmin()
        {
            var transaction = await CreateData();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/transactions/{transaction.Id}");
            request.Headers.Add("Authorization", $"Bearer {AdminToken}");

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWhenUserOwnsTransaction()
        {
            var transaction = await CreateData();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/transactions/{transaction.Id}");
            request.Headers.Add("Authorization", $"Bearer {TestToken}");

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_ShouldReturnForbiddenWhenUserDoesNotOwnTransaction()
        {
            var transaction = await CreateData();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/transactions/{transaction.Id}");
            request.Headers.Add("Authorization", $"Bearer {Test2Token}");

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Order_ShouldReturnUnauthorizedWhenNotAuthenticated()
        {
            var response = await client.PostAsync("api/transactions/order", new StringContent(""));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Order_ShouldReturnBadRequestWhenNoContent()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/order");
            request.Headers.Add("Authorization", $"Bearer {TestToken}");
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Order_ShouldReturnOKWhenTransactionCreated()
        {
            var item = await CreateItemData();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/order");
            request.Headers.Add("Authorization", $"Bearer {TestToken}");
            request.Content = new StringContent(JsonConvert.SerializeObject(new List<PurchaseOrderItemDTO>
            {
                new PurchaseOrderItemDTO
                {
                    ItemId = item.Id,
                    Quantity = 5
                }
            }), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<Item> CreateItemData()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/items/add");
            request.Headers.Add("Authorization", $"Bearer {AdminToken}");
            request.Content = new StringContent(JsonConvert.SerializeObject(new Item
            {
                Name = "Item" + Guid.NewGuid(),
                Description = "Description",
                Price = 9.99,
                Quantity = 10
            }), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<Item>(responseContent);
            return item;
        }

        private async Task<Transaction> CreateData()
        {
            var item = await CreateItemData();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/order");
            request.Headers.Add("Authorization", $"Bearer {TestToken}");
            request.Content = new StringContent(JsonConvert.SerializeObject(new List<PurchaseOrderItemDTO>
            {
                new PurchaseOrderItemDTO
                {
                    ItemId = item.Id,
                    Quantity = 5
                }
            }), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var transaction = JsonConvert.DeserializeObject<Transaction>(responseContent);
            return transaction;
        }
    }
}
