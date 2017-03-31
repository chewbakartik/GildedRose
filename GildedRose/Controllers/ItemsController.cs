using System;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Controllers
{
    [Route("api/items")]
    public class ItemsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}