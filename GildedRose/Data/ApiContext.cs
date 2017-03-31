using GildedRose.Models;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) {}

        public DbSet<Item> Items { get; set; }
    }
}