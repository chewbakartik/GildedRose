using System.Collections.Generic;
using System.Linq;
using GildedRose.Data;
using GildedRose.Data.Abstract;
using GildedRose.Data.Repositories;
using GildedRose.Data.Services;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GildedRose.Tests.Integration.Services
{
    public class TransactionServiceTest
    {
        [Fact]
        public void Create_ShouldSaveTransactionAndDetailsWhenInvoked()
        {
            var totalQuantity = 10;
            var purchaseQuantity = 5;
            var price = 4.99;

            var options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase(databaseName: "Create_ShouldSaveTransactionAndDetailsWhenInvoked")
                .Options;
            using (var context = new ApiContext(options))
            {
                // initialize objects
                var itemRepository = new ItemRepository(context);
                var transactionDetailRepository = new TransactionDetailRepository(context);
                var transactionRepository = new TransactionRepository(context);
                var userRepository = new UserRepository(context);
                var service = new TransactionService(itemRepository, transactionDetailRepository, transactionRepository);

                // add data
                var item = new Item
                {
                    Name = "Item1",
                    Description = "Description1",
                    Price = price,
                    Quantity = totalQuantity
                };
                itemRepository.Add(item);
                itemRepository.Commit();
                var user = new User
                {
                    AuthId = "auth|test-user",
                    Email = "test@test.test"
                };
                userRepository.Add(user);
                userRepository.Commit();

                // preform action
                var transaction = service.Create(new List<PurchaseOrderItemDTO>
                {
                    new PurchaseOrderItemDTO {ItemId = item.Id, Quantity = purchaseQuantity}
                }, user.Id);
            }

            using (var context = new ApiContext(options))
            {
                // initialize objects
                var itemRepository = new ItemRepository(context);
                var transactionDetailRepository = new TransactionDetailRepository(context);
                var transactionRepository = new TransactionRepository(context);
                var userRepository = new UserRepository(context);
                var service = new TransactionService(itemRepository, transactionDetailRepository, transactionRepository);


                var transactions = transactionRepository.AllProperties();
                Assert.NotEmpty(transactions);
                Assert.Equal(1, transactions.Count());
                var transaction = transactions.FirstOrDefault();
                Assert.Equal(price * purchaseQuantity, transaction.Total);
                Assert.Equal(1, transaction.Details.Count);
                // Validate that quantity of Item changed in inventory
                Assert.Equal(totalQuantity - purchaseQuantity, transaction.Details.FirstOrDefault().Item.Quantity);
            }
        }
    }
}
