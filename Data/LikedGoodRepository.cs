using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ShopProject.Data
{
    public class LikedGoodRepository: GenericRepository<LikedGood>, ILikedGoodRepository
    {
        ShopContext context;
        public LikedGoodRepository(ShopContext context): base(context)
        {
            this.context = context;
        }

        public async Task<LikedGood> GetByGoodAndUserId(int goodid, int userid)
        {
            return await context.LikedGoods.FirstOrDefaultAsync(l => l.GoodId == goodid && l.UserId == userid);
        }

        public async Task<List<LikedGood>> GetAllByUserId(int userid)
        {
            return await context.LikedGoods.Where(g => g.UserId == userid).ToListAsync();
        }
    }
}
