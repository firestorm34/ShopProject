using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ShopProject.Data
{
    public class HistoryRepository : GenericRepository<History>, IHistoryRepository
    {
        ShopContext context;
        public HistoryRepository(ShopContext context) : base(context)
        {
            this.context = context;
        }
        
        public async Task<History> GetByUserId(int userid)
        {
            var history =await context.Histories.FirstOrDefaultAsync(h => h.UserId == userid);
            if(history == null)
            {
                history = new History { UserId = userid };
                context.Histories.Add(history);
                await context.SaveChangesAsync();
            }
            return history;
        }
    }
}
