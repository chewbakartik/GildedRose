using GildedRose.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GildedRose.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) {}

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            //Items
            modelBuilder.Entity<Item>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Item>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Item>().Property(p => p.Description).HasMaxLength(500);
        }
    }
}