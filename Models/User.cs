using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ShopProject.Models
{
    public partial class User: IdentityUser<int>
    {
        public User()
        {
            Histories = new HashSet<History>();
            LikedGoods = new HashSet<LikedGood>();
            Orders = new HashSet<Order>();
        }

        [Key]
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string City { get; set; }

        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<LikedGood> LikedGoods { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Basket Basket { get; set; }
    }
}
