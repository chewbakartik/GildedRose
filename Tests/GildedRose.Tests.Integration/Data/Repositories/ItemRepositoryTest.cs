using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GildedRose.Data;
using GildedRose.Data.Repositories;
using GildedRose.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GildedRose.Tests.Integration.Data.Repositories
{
    public class ItemRepositoryTest
    {
        protected DbContextOptions<ApiContext> Options { get; set; }

        public ItemRepositoryTest()
        {
            Options = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase()
                .Options;
        }

        [Fact]
        public void CanSaveAndFetchItem()
        {
            var item = GenerateItem();
            using (var context = new ApiContext(Options))
            {
                var itemRepository = new ItemRepository(context);
                itemRepository.Add(item);
                itemRepository.Commit();

                var itemReturned = itemRepository.Get(item.Id);
                Assert.Equal(item, itemReturned);
            }
        }

        [Fact]
        public void CanFetchItemByPredicate()
        {
            var item = GenerateItem();
            using (var context = new ApiContext(Options))
            {
                var itemRepository = new ItemRepository(context);
                itemRepository.Add(item);
                itemRepository.Commit();

                var itemReturned = itemRepository.Get(i => i.Description == item.Description);
                Assert.Equal(item, itemReturned);
            }
        }

        [Fact]
        public void CanFindMultipleItemsByPredicate()
        {
            var item = GenerateItem();
            var item2 = GenerateItem();
            var item3 = GenerateItem();
            item2.Price = 25.00;
            item3.Price = 15.00;

            using (var context = new ApiContext(Options))
            {
                var itemRepository = new ItemRepository(context);
                itemRepository.Add(item);
                itemRepository.Add(item2);
                itemRepository.Add(item3);
                itemRepository.Commit();

                var items = itemRepository.FindBy(i => i.Price >= 11.00);
                Assert.NotNull(items);
                Assert.Equal(2, items.Count());
                Assert.DoesNotContain(item, items);
                Assert.Contains(item2, items);
                Assert.Contains(item3, items);
            }
        }

        [Fact]
        public void CanEditItem()
        {
            var item = GenerateItem();
            using (var context = new ApiContext(Options))
            {
                var itemRepository = new ItemRepository(context);
                itemRepository.Add(item);
                itemRepository.Commit();

                var returnedItem = itemRepository.Get(item.Id);
                Assert.Equal(10, returnedItem.Quantity);

                item.Quantity = 5;
                itemRepository.Edit(item);
                itemRepository.Commit();

                returnedItem = itemRepository.Get(item.Id);
                Assert.Equal(5, returnedItem.Quantity);
            }
        }

        [Fact]
        public void CanDeleteItem()
        {
            var item = GenerateItem();
            using (var context = new ApiContext(Options))
            {
                var itemRepository = new ItemRepository(context);
                itemRepository.Add(item);
                itemRepository.Commit();

                var returnedItem = itemRepository.Get(item.Id);
                Assert.NotNull(returnedItem);

                itemRepository.Delete(item);
                itemRepository.Commit();

                returnedItem = itemRepository.Get(item.Id);
                Assert.Null(returnedItem);
            }
        }

        private Item GenerateItem()
        {
            return new Item
            {
                Name = "Item-" + Guid.NewGuid(),
                Description = "Description-" + Guid.NewGuid(),
                Price = 9.99,
                Quantity = 10
            };
        }
    }
}
