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
       
       
        public User GetByUserName(string Name);
        public  Task<User> CheckEmail(string Email);

        /// <summary>
        /// Trying to find user with defined password and Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns> User, that was found or null</returns>
        /// <typeparam name="model">The type used for the primary key for the user.</typeparam>
        public Task<User> TryLogin(LoginModel model);

    }
}
