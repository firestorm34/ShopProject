using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopProject.Data.Interfaces;
using ShopProject.Models;


namespace ShopProject.Data
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public ShopContext context;

        public UserRepository(ShopContext context) : base(context)
        {
            this.context = context;
        }
        

        public User GetByUserName(string Name)
        {
            return context.Users.FirstOrDefault(u => u.UserName == Name);
        }



        public async Task<User> TryLogin(LoginModel model)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email/* && u.Password == model.Password*/);
        }

        public async Task<User> CheckEmail( string Email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }
    }
}
