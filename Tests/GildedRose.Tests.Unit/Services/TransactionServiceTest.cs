using System;
using System.Collections.Generic;
using GildedRose.Data.Abstract;
using GildedRose.Data.Services;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;
using Moq;
using Xunit;

namespace GildedRose.Tests.Unit.Services
{
    public class TransactionServiceTest
    {
        [Fact]
        public void Validate_ReturnsFalseWhenItemDoesNotExist()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.Get(1)).Returns<Item>(null);
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionDetailRepository = new Mock<ITransactionDetailRepository>();
            var userRepository = new Mock<IUserRepository>();
            int userId = 1;
            userRepository.Setup(p => p.Get(1)).Returns(new User
            {
                Id = userId,
                AuthId = "Auth0UserId"
            });
            var messages = new List<String>();
            var service = new TransactionService(itemRepository.Object, transactionDetailRepository.Object, transactionRepository.Object);
            var items = new List<PurchaseOrderItemDTO>();
            items.Add(new PurchaseOrderItemDTO{ ItemId = 1, Quantity = 5 });
            var valid = service.Validate(items, out messages);
            Assert.False(valid);
            Assert.Equal(1, messages.Count);
            Assert.Equal("Item 1: Does not exist", messages[0]);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenItemQuantityIsLessThanOrdered()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.Get(1)).Returns(new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 2
            });
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionDetailRepository = new Mock<ITransactionDetailRepository>();
            var userRepository = new Mock<IUserRepository>();
            int userId = 1;
            userRepository.Setup(p => p.Get(1)).Returns(new User
            {
                Id = userId,
                AuthId = "Auth0UserId"
            });
            var messages = new List<String>();
            var service = new TransactionService(itemRepository.Object, transactionDetailRepository.Object, transactionRepository.Object);
            var items = new List<PurchaseOrderItemDTO>();
            items.Add(new PurchaseOrderItemDTO { ItemId = 1, Quantity = 5 });
            var valid = service.Validate(items, out messages);
            Assert.False(valid);
            Assert.Equal(1, messages.Count);
            Assert.Equal("Item 1: Ordering 5 quantity, but only 2 are available", messages[0]);
        }

        [Fact]
        public void Validate_ReturnsFalseAndStacksErrorMessageWhenMultipleItemsOrderedAreNotValid()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.Get(1)).Returns(new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 2
            });
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionDetailRepository = new Mock<ITransactionDetailRepository>();
            var userRepository = new Mock<IUserRepository>();
            int userId = 1;
            userRepository.Setup(p => p.Get(1)).Returns(new User
            {
                Id = userId,
                AuthId = "Auth0UserId"
            });
            var messages = new List<String>();
            var service = new TransactionService(itemRepository.Object, transactionDetailRepository.Object, transactionRepository.Object);
            var items = new List<PurchaseOrderItemDTO>();
            items.Add(new PurchaseOrderItemDTO { ItemId = 1, Quantity = 5 });
            items.Add(new PurchaseOrderItemDTO { ItemId = 2, Quantity = 1 });
            var valid = service.Validate(items, out messages);
            Assert.False(valid);
            Assert.Equal(2, messages.Count);
            Assert.Equal("Item 1: Ordering 5 quantity, but only 2 are available", messages[0]);
            Assert.Equal("Item 2: Does not exist", messages[1]);
        }

        [Fact]
        public void Validate_ReturnsTrueWhenItemExistsAndItemQuantityIsGreaterThanOrdered()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.Get(1)).Returns(new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 10
            });
            var transactionDetailRepository = new Mock<ITransactionDetailRepository>();
            var transactionRepository = new Mock<ITransactionRepository>();
            var userRepository = new Mock<IUserRepository>();
            int userId = 1;
            userRepository.Setup(p => p.Get(1)).Returns(new User
            {
                Id = userId,
                AuthId = "Auth0UserId"
            });
            var messages = new List<String>();
            var service = new TransactionService(itemRepository.Object, transactionDetailRepository.Object, transactionRepository.Object);
            var items = new List<PurchaseOrderItemDTO>();
            items.Add(new PurchaseOrderItemDTO { ItemId = 1, Quantity = 5 });
            var valid = service.Validate(items, out messages);
            Assert.True(valid);
            Assert.Equal(0, messages.Count);
        }
    }
}
