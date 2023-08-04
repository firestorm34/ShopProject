//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ShopProject.Models;

//namespace ShopProject.Data
//{
//    public  class CurrentUser
//    {
//        public User user = new User() { Id = 6, Email = "12", Name = "12", /*Password = "12",*/ LastName = "12", City = "12" };
//        ShopContext context;
//        public int AmountOfGoodInBasket
//        {
//            get => context.Baskets.FirstOrDefault(b => b.UserId == user.Id).AmountOfGood;
//            set => AmountOfGoodInBasket = value;
//        }

//        public CurrentUser(ShopContext _context)
//        {
//            context = _context;
//        }

//    }
//}
