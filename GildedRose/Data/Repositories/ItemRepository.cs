using GildedRose.Data.Abstract;
using GildedRose.Models;

namespace GildedRose.Data.Repositories
{
    public class ItemRepository : EntityBaseRepository<Item>, IItemRepository
    {
        public ItemRepository(ApiContext context) : base(context) {}
    }
}