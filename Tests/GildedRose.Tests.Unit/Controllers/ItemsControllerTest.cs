using GildedRose.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GildedRose.Tests.Unit.Controllers
{
    public class ItemsControllerTest
    {
        [Fact]
        public void Index_ReturnsOkRequest()
        {
            var controller = new ItemsController();

            var result = controller.Index();

            Assert.IsType<OkResult>(result);
        }
    }
}