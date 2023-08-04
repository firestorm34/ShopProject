using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Models
{
    public class BasketViewModel
    {
        public List<string> GoodNames { get; set; } = new List<string>();
        public List<int> GoodAmounts { get; set; } = new List<int>();
        public List<decimal> SumOfGoods { get; set; } = new List<decimal>();
        public int AmountOfGoods { get; set; }
        public decimal TotalSum { get; set; }
        public List<decimal> GoodPrices { get; set; } = new List<decimal>();
        public int UserId { get; set; }
        public List<int> GoodId { get; set; } = new List<int>();
        public List<bool> IsAvailable { get; set; } = new List<bool>();
        public bool CanBuy { get; set; }
    }
}
