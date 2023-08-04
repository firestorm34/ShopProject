using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class HistoryElement
    {
        public int Id { get; set; }
        public int HistoryId { get; set; }
        public int ViwedGoodId { get; set; }

        public virtual History History { get; set; }
        public virtual Good ViwedGood { get; set; }
    }
}
