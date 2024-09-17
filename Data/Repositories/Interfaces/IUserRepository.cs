using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        public Task<User> GetById(int id);
       


    }
}
