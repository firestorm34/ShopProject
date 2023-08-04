using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class LikedGood
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GoodId { get; set; }

        public virtual Good Good { get; set; }
        public virtual User User { get; set; }
    }
}
