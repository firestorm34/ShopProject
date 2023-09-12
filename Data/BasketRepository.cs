using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopProject.Data.Interfaces;
using ShopProject.Models;
namespace ShopProject.Data
{
    public class BasketRepository: GenericRepository<Basket>, IBasketRepository
    {
        public ShopContext context;

        public BasketRepository (ShopContext context):base(context)
        {
            this.context = context;
        }

        public async Task<Basket> GetByIdFromOrder(int basket_id)
        {
            return  await context.Baskets.Include(b=>b.User).FirstOrDefaultAsync(b => b.Id == basket_id && b.IsInOrder == true);

        }

        public Task<Basket> GetByUserId(int id)
        {
            return context.Baskets.Where(b => b.IsInOrder == false).FirstOrDefaultAsync(p => p.UserId == id);
        }
    }
}
