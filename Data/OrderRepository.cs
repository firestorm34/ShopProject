using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
using ShopProject.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShopProject.Data
{
    public class OrderRepository:  GenericRepository<Order>, IOrderRepository
    {
        ShopContext context;
        public  OrderRepository(ShopContext context): base(context)
        {
            this.context = context;
        }

        public override async Task<List<Order>> GetAll()
        {
            return await context.Orders.Include(o => o.User).ToListAsync();
        }

        public override Task<Order> Get(int id)
        {
            return context.Orders.Include(o=>o.User).FirstOrDefaultAsync(o=>o.Id == id);
        }
    }
}
