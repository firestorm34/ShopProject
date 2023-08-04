using System;
using System.Collections.Generic;

#nullable disable

namespace ShopProject.Models
{
    public partial class Category
    {
        public Category()
        {
            Goods = new HashSet<Good>();
            InverseParentCategory = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
        public virtual ICollection<Category> InverseParentCategory { get; set; } = null;

        public static implicit operator List<object>(Category v)
        {
            throw new NotImplementedException();
        }
    }
}
