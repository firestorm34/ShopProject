using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopProject.Models;

namespace ShopProject.Data
{
    public class GoodRepository : GenericRepository<Good>, IGoodRepository
    {
        private readonly ShopContext db;
        public GoodRepository(ShopContext context) : base(context)
        {
            db = context;
        }

        public async override Task<Good> GetAsync(int id)
        {

            var d = await db.Goods.Include(good => good.Manufacturer).Include(good => good.Photos).
                Include(g => g.GoodAtStock).Where(g => g.IsArchieved == false).FirstOrDefaultAsync(g => g.Id == id);
            // to load navigation entry -> db.Entry(db.Goods.Find(id)).Navigation("Manufacturer").Load();
            return d;

        }


        public async override Task<List<Good>> GetAllAsync()
        {
            var e = db.Goods.Include(good => good.Manufacturer).Include(good => good.Photos)
                 .Include(g => g.GoodAtStock).Where(g => g.IsArchieved == false).AsEnumerable();
            e = e.Distinct(new GoodComparerDistinct());

            return e.ToList();
        }

        public async Task<List<Good>> GetByNameAsync(string name)
        {
            var e =  await db.Goods.Where(c => c.Name.Contains(name)).ToListAsync();
            
            return e;
        }
        

    }
}
