using System;
using GildedRose.Entities;

namespace GildedRose.Models
{
    public class Item : IEntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}