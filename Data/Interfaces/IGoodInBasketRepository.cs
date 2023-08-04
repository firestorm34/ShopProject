using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface IGoodInBasketRepository : IGenericRepository<GoodInBasket>
    {
        
       public  Task<GoodInBasket> GetByGoodAndBasketId(int goodid, int basketid);
        public IEnumerable<GoodInBasket> GetAllByBasketId(int id);



    }
}
