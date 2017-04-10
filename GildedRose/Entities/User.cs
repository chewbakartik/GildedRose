namespace GildedRose.Entities
{
    public class User : IEntityBase
    {
        public string AuthId { get; set; }
        public string Email { get; set; }
    }
}
