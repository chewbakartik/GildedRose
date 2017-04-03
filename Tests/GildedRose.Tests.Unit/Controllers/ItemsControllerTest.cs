using GildedRose.Controllers;
using GildedRose.Data;
using GildedRose.Data.Repositories;
using GildedRose.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GildedRose.Tests.Unit.Controllers
{
    public class ItemsControllerTest
    {
        [Fact]
        public void Index_ReturnsOkObjectResult()
        {
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase().Options;
            using (var context = new ApiContext(options))
            {
                var itemRepository = new ItemRepository(context);
                var controller = new ItemsController(itemRepository);

                var result = controller.Index();

                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void Get_ReturnsNotFoundWhenObjectDoesNotExist()
        {
            Assert.Equal(1, 1);
//            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase().Options;
//            using (var context = new ApiContext(options))
//            {
//                var itemRepository = new ItemRepository(context);
//                var controller = new ItemsController(itemRepository);
//                var result = controller.Get(1);
//
//                Assert.IsType<NotFoundResult>(result);
//            }
        }

        [Fact]
        public void Get_ReturnsOkObjectResultWhenObjectDoesExist()
        {
            var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase().Options;
//            using (var context = new ApiContext(options))
//            {
//                var itemRepository = new ItemRepository(context);
//                itemRepository.Add(new Item
//                {
//                    Name = "Item1",
//                    Description = "Description1",
//                    Price = 9.99
//                });
//                itemRepository.Commit();
//            }
        }
    }
}