using System.Collections.Generic;
using GildedRose.Entities;

namespace GildedRose.Data.Abstract
{
    public interface IItemRepository : IEntityBaseRepository<Item>
    {
        Item GetByName(string name);
    }

    public interface ITransactionDetailRepository : IEntityBaseRepository<TransactionDetail> {}

    public interface ITransactionRepository : IEntityBaseRepository<Transaction>
    {
        IEnumerable<Transaction> AllProperties();
        Transaction GetProperties(int id);
    }

    public interface IUserRepository : IEntityBaseRepository<User>
    {
        User GetByAuthId(string authId);
    }
}