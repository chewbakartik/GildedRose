using GildedRose.Data.Abstract;
using GildedRose.Entities;

namespace GildedRose.Data.Repositories
{
    public class TransactionDetailRepository : EntityBaseRepository<TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(ApiContext context) : base(context) {}
    }
}