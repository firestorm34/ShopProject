using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface IHistoryRepository: IGenericRepository<History>
    {/// <summary>
     /// Returns History by userid. If History asociated with that UserId doesn't exist, creates new History.
     /// </summary>
     /// <param name="userid"></param>
     /// 
     /// <returns></returns>
        public Task<History> GetByUserId(int userid);
    }
}
