using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Goods = new HashSet<Good>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}
