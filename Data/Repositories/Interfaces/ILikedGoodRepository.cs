using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface ILikedGoodRepository: IGenericRepository<LikedGood>
    {

        public Task<LikedGood> GetByGoodAndUserId(int goodid, int userid);
        public  Task<List<LikedGood>> GetAllByUserId(int userid);
    }
}
