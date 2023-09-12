using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Data
{
    public interface IGoodRepository: IGenericRepository<Good>
    {

        public Task<List<Good>> GetByNameAsync(string name);
    }
}
