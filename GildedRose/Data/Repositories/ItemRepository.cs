using GildedRose.Data.Abstract;
using GildedRose.Entities;

namespace GildedRose.Data.Repositories
{
    public class ItemRepository : EntityBaseRepository<Item>, IItemRepository
    {
        public ItemRepository(ApiContext context) : base(context) {}

        public Item GetByName(string name)
        {
            return this.Get(x => x.Name == name);
        }
    }
}