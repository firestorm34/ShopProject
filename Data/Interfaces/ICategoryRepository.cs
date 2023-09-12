using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        public  List<Category> GetChildCategories(int id);

        public   Task<List<Category>> GetAllForManageAsync();

        public List<Category> GetByParentCategory(int id);


    }
}
