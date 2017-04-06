using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Entities
{
    public class User : IEntityBase
    {
        public string AuthId { get; set; }
        public string Email { get; set; }
    }
}
