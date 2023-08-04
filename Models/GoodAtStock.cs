using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class GoodAtStock
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public int AmountLeft { get; set; }

        public virtual Good Good { get; set; }
    }
}
