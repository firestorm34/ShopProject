using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public byte[] Image { get; set; }

        public Good Good { get; set; }
    }
}
