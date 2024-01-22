using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
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

        public  void IncreaseGoodAmount(int goodid, int basketid)
        {
            
            GoodInBasket goodInBasket = context.GoodInBaskets.FirstOrDefault(g => g.GoodId == goodid && g.BasketId== basketid);
            if (goodInBasket != null )
            {
                goodInBasket.Amount++;
                Update(goodInBasket);
            }

        }
        public void DecreaseGoodAmount(int goodid, int basketid)
        {

            GoodInBasket goodInBasket = context.GoodInBaskets.FirstOrDefault(g => g.GoodId == goodid && g.BasketId == basketid);
            if (goodInBasket != null)
            {
                goodInBasket.Amount--;
                Update(goodInBasket);
            }

        }
        public  IEnumerable<GoodInBasket> GetAllByBasketId(int id)
        {
            return  context.GoodInBaskets.Include(g => g.Good).Where(g => g.BasketId == id);
        }
    }
}
