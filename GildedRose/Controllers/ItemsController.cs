using GildedRose.Data.Abstract;
using GildedRose.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        [HttpGet("{id}", Name = "GetItem")]
        public IActionResult Get(int id)
        {
            var item = _itemRepository.Get(id);
            if (item != null)
            {
                return new OkObjectResult(item);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public IActionResult Add([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest("");
            }
            var itemExist = _itemRepository.GetByName(item.Name);
            if (itemExist != null)
            {
                return BadRequest("Item already exists");
            }
            _itemRepository.Add(item);
            _itemRepository.Commit();
            return CreatedAtRoute("GetItem", new {item.Id}, item);
        }
    }
}