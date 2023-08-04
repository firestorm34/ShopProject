using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int UserId { get; set; }
        public int Adress { get; set; }
        public Status Status { get; set; }
        public decimal Cost { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual User User { get; set; }
    }

    public enum Status
    {
        Delivered,
        InProcess
    }
}
