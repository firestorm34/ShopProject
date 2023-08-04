//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ShopProject.Models;
//using Microsoft.AspNetCore.Identity;
//using ShopProject.ViewModels;
//using Microsoft.Extensions.Options;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using ShopProject.Data;
//using ShopProject.Models;


//namespace ShopProject.Identity
//{
//    public class MyUserManager: UserManager<User>
//    {
//        public MyUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
//            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, 
//            ILookupNormalizer keyNormalizer,
//            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
//            :base(
//              new UserStore<User>(new ShopContext(optionsAccessor)),
//              new CustomOptions(),
//              new PasswordHasher<ApplicationUser>(),
//              new UserValidator<ApplicationUser>[] { new UserValidator<ApplicationUser>() },
//              new PasswordValidator[] { new PasswordValidator() },
//              new UpperInvariantLookupNormalizer(),
//              new IdentityErrorDescriber(),
//              services,
//              logger)
//        {

//        }
//    }
//}
