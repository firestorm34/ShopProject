using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopProject.Data.Interfaces;
using ShopProject.Models;

namespace ShopProject.Data
{
    public class GoodInBasketRepository: GenericRepository<GoodInBasket>, IGoodInBasketRepository
    {
        ShopContext context;
        public GoodInBasketRepository(ShopContext context): base(context)
        {
            this.context = context;
        }

        //public async Task<GoodInBasket> GetByBasketId(int id)
        //{
        //    return await context.GoodInBaskets.FirstOrDefaultAsync(g => g.BasketId == id);
        //}
        public async Task<GoodInBasket> GetByGoodAndBasketId(int goodid, int basketid)
        {
            return await context.GoodInBaskets.Include(g=>g.Good).
                FirstOrDefaultAsync(g => g.GoodId == goodid &&g.BasketId == basketid);


        }

        public  IEnumerable<GoodInBasket> GetAllByBasketId(int id)
        {
            return  context.GoodInBaskets.Include(g => g.Good).Where(g => g.BasketId == id);
        }
    }
}
