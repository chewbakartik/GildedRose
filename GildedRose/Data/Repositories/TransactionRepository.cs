using GildedRose.Data.Abstract;
using GildedRose.Entities;

namespace GildedRose.Data.Repositories
{
    public class TransactionRepository : EntityBaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApiContext context) : base(context) {}
    }
}