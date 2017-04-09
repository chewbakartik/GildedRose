using System.Collections.Generic;
using System.Linq;
using GildedRose.Data.Abstract;
using GildedRose.Entities;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Data.Repositories
{
    public class TransactionRepository : EntityBaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApiContext context) : base(context) {}

        public virtual IEnumerable<Transaction> AllProperties()
        {
            IQueryable<Transaction> query = _context.Set<Transaction>();
            query = query.Include(t => t.User)
                .Include(t => t.Details)
                .ThenInclude(d => d.Item);
            return query.AsEnumerable();
        }

        public virtual Transaction GetProperties(int id)
        {
            IQueryable<Transaction> query = _context.Set<Transaction>();
            query = query.Include(t => t.User)
                .Include(t => t.Details)
                .ThenInclude(d => d.Item);
            return query.FirstOrDefault(x => x.Id == id);
        } 
    }
}