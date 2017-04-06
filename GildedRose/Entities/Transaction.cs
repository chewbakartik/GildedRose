using System;
using System.Collections.Generic;

namespace GildedRose.Entities
{
    public class Transaction : IEntityBase
    {
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TransactionDetail> Details { get; set; }
    }
}
