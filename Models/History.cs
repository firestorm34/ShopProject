using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class History
    {
        public History()
        {
            HistoryElements = new HashSet<HistoryElement>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int AmountOfViwedGoods { get; set; }
        public int Year { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<HistoryElement> HistoryElements { get; set; }
    }
}
