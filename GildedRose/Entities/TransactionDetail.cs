using Newtonsoft.Json;

namespace GildedRose.Entities
{
    public class TransactionDetail : IEntityBase
    {
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SubTotal { get; set; }

        public virtual Item Item { get; set; }

        [JsonIgnore]
        public virtual Transaction Transaction { get; set; }
    }
}
