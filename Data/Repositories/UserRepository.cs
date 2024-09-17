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
        
        public  Task<User> GetById(int id)
        {
            return  context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }


    }
}
