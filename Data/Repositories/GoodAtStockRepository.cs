using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data.Interfaces;
using ShopProject.Models;
using Microsoft.EntityFrameworkCore;
namespace ShopProject.Data
{
    public class GoodAtStockRepository : GenericRepository<GoodAtStock>, IGoodAtStockRepository
    {
        ShopContext context;
        public GoodAtStockRepository(ShopContext context): base(context)
        {
            this.context = context;
        }

        public async Task<GoodAtStock> GetByGoodId(int id)
        {
            
            return await context.GoodAtStocks.FirstOrDefaultAsync(g => g.GoodId == id);
        }
    }
}
