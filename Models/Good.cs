using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class Good
    {
        public Good()
        {
            
            GoodInBaskets = new HashSet<GoodInBasket>();
            HistoryElements = new HashSet<HistoryElement>();
            LikedGoods = new HashSet<LikedGood>();
            Photos = new HashSet<Photo>();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int CategoryId { get; set; }
        public byte[] MainImage { get; set; }

        public bool IsArchieved { get; set; }


        public virtual Category Category { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual GoodAtStock GoodAtStock { get; set; }
        public virtual ICollection<GoodInBasket> GoodInBaskets { get; set; }
        public virtual ICollection<HistoryElement> HistoryElements { get; set; }
        public virtual ICollection<LikedGood> LikedGoods { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

    }
}
