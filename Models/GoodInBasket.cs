using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class GoodInBasket
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int GoodId { get; set; }
        public int Amount { get; set; }
        public decimal SumOfGoods { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual Good Good { get; set; }
    }
}
