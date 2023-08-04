using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data.Interfaces;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace ShopProject.Data
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        IQueryable<Category> finalSubcategories = null;
        ShopContext context;
        public CategoryRepository(ShopContext context) : base(context)
        {
            this.context = context;
        }


        public async override Task<Category> Get(int id)
        {

            return await context.Categories.FindAsync(id);
        }

        public async override Task<List<Category>> GetAll()
        {
            var a = context.Categories.Where(g => g.Id != 0).Include(c => c.ParentCategory);
            return await a.ToListAsync();
        }
        public async Task<List<Category>> GetAllForManage()
        {
            var a = context.Categories.Include(c => c.ParentCategory);
            return await a.ToListAsync();
        }

        public List<Category> GetByParentCategory(int id)
        {
            return context.Categories.Where(c=>c.Id !=0).Where(c => c.ParentCategoryId ==id).ToList();
        }
        public  List<Category> GetChildCategories(int id)
        {
            
            if (id == 0)
            {
                return null;
            }
            var subcategories =  context.Categories.Where(c => c.ParentCategoryId == id);
            if (subcategories != null)
            {
                
                foreach (var subcategory in subcategories.ToList())
                {
                    var subcategorieschild = GetChildCategories(subcategory.Id);
                    if (subcategorieschild != null)
                    {
                        finalSubcategories = subcategories.Concat(subcategorieschild);
                    }
                }

                // foreach
                return (List<Category>)finalSubcategories;
            }
            return null;

        }
    }
}
