using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data;

namespace ShopProject.Data.Interfaces
{
    public interface IGoodAtStockRepository: IGenericRepository<GoodAtStock>
    {
        public Task<GoodAtStock> GetByGoodId(int id);
    }
}
