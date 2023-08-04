using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data.Interfaces;
using ShopProject.Models;

namespace ShopProject.Data
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public ShopContext context;
        public AdminRepository( ShopContext context): base(context)
        {
            this.context = context;
        }
    }
}
