using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Data
{
    public class GoodComparerDistinct : IEqualityComparer<Good>
    {

        public bool Equals(Good x, Good y)
        {
            return x.Id == y.Id;

        }

        public int GetHashCode(Good obj)
        {
            return obj == null ? 0 : obj.Id;
        }
    }
}
