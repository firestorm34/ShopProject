using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface IBasketRepository: IGenericRepository<Basket>
    {

        public Task<Basket> GetByUserId(int id);
        public Task<Basket> GetByIdFromOrder(int basket_id);


    }
}
