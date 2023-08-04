using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class Basket
    {
        public Basket()
        {
            GoodInBaskets = new HashSet<GoodInBasket>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int AmountOfGood { get; set; }
        public decimal TotalSum { get; set; }
        public int UserId { get; set; }
        public bool IsInOrder { get; set; }

        public virtual ICollection<GoodInBasket> GoodInBaskets { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual User User { get; set; }
    }

   
}
