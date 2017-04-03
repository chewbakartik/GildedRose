using System;
using GildedRose.Data.Abstract;
using GildedRose.Entities;
using GildedRose.Models;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Controllers
{
    [Route("api/items")]
    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new OkObjectResult(_itemRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _itemRepository.Get(id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return NotFound();
        }
    }
}