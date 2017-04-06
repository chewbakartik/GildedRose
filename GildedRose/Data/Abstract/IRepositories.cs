using GildedRose.Entities;

namespace GildedRose.Data.Abstract
{
    public interface IItemRepository : IEntityBaseRepository<Item>
    {
        Item GetByName(string name);
    }

    public interface ITransactionDetailRepository : IEntityBaseRepository<TransactionDetail> {}

    public interface ITransactionRepository : IEntityBaseRepository<Transaction> {}

    public interface IUserRepository : IEntityBaseRepository<User>
    {
        User GetByAuthId(string authId);
    }
}