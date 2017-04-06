using System.Collections.Generic;
using System.Linq;
using GildedRose.Controllers;
using GildedRose.Data.Abstract;
using GildedRose.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GildedRose.Tests.Unit.Controllers
{
    public class ItemsControllerTest
    {
        [Fact]
        public void Index_ReturnsOkObjectResult()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.GetAll()).Returns(new List<Item>());
            var controller = new ItemsController(itemRepository.Object);

            var result = controller.Index();

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var items = okObjectResult.Value as IEnumerable<Item>;
            Assert.NotNull(items);
            Assert.Equal(0, items.Count());
        }

        [Fact]
        public void Get_ReturnsNotFoundWhenObjectDoesNotExist()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.Get(1)).Returns<Item>(null);
            var controller = new ItemsController(itemRepository.Object);
            var result = controller.Get(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_ReturnsOkObjectResultWhenObjectDoesExist()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.Get(1)).Returns(new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 10
            });
            var controller = new ItemsController(itemRepository.Object);
            var result = controller.Get(1);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var item = okObjectResult.Value as Item;
            Assert.NotNull(item);
            Assert.Equal("Item1", (item.Name));
            Assert.Equal("Description1", item.Description);
            Assert.Equal(9.99, item.Price);
            Assert.Equal(10, item.Quantity);
        }

        [Fact]
        public void Add_ReturnsBadRequestWhenItemIsNull()
        {
            var itemRepository = new Mock<IItemRepository>();
            var controller = new ItemsController(itemRepository.Object);
            Item item = null;
            var result = controller.Add(item);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Add_ReturnsBadRequestWhenItemAlreadyExists()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.GetByName("Item1")).Returns(new Item
            {
                Id = 1,
                Name = "Item1",
                Description = "Description1",
                Price = 9.99,
                Quantity = 10
            });
            var controller = new ItemsController(itemRepository.Object);
            Item item = new Item {
                Name = "Item1",
                Description = "Description1",
                Price = 19.99,
                Quantity = 100
            };
            var result = controller.Add(item);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Add_ReturnsCreatedAtRouteRequestWhenItemAddedSuccessfully()
        {
            var itemRepository = new Mock<IItemRepository>();
            itemRepository.Setup(p => p.GetByName("Item1")).Returns<Item>(null);
            var controller = new ItemsController(itemRepository.Object);
            Item item = new Item
            {
                Name = "Item1",
                Description = "Description1",
                Price = 19.99,
                Quantity = 10
            };
            var result = controller.Add(item);
            Assert.IsType<CreatedAtRouteResult>(result);
        }
    }
}