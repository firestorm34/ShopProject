//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using ShopProject.Data.Interfaces;
//using ShopProject.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;

//namespace ShopProject.Data
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        ShopContext context;
//        SignInManager<User> signInManager;
//        UserManager<User> userManager;
//        #region Variables for repositories
//        IGoodRepository goodRepository;
//        IManufactureRepository manufacturerRepository;
//        IOrderRepository orderRepository;
//        IUserRepository userRepository;
//        IGoodInBasketRepository goodInBasketRepository;
//        IBasketRepository basketRepository;
//        IAdminRepository adminRepository;
//        ICategoryRepository categoryRepository;
//        IGoodAtStockRepository goodAtStockRepository;
//        IHistoryRepository historyRepository;
//        IHistoryElementRepository historyElementRepository;
//        ILikedGoodRepository likedGoodRepository;

//        #endregion

//        public UnitOfWork(ShopContext _context, SignInManager<User> _signInManager, UserManager<User> _userManager)
//        {
//            context = _context;
//            signInManager = _signInManager;
//            userManager = _userManager;
//        }


//        #region Properties for repositories
//        public IUserRepository UserRepository { get => userRepository == null ? new UserRepository(context) : userRepository; }
//        public IGoodInBasketRepository GoodInBasketRepository
//        {
//            get => goodInBasketRepository == null ?
//new GoodInBasketRepository(context) : goodInBasketRepository;
//        }
//        public IBasketRepository BasketRepository { get => basketRepository == null ? new BasketRepository(context) : basketRepository; }
//        public IAdminRepository AdminRepository { get => adminRepository == null ? new AdminRepository(context) : adminRepository; }
//        public ICategoryRepository CategoryRepository { get => categoryRepository == null ? new CategoryRepository(context) : categoryRepository; }
//        public IGoodAtStockRepository GoodAtStockRepository
//        {
//            get => goodAtStockRepository == null ?
//new GoodAtStockRepository(context) : goodAtStockRepository;
//        }

//        public ILikedGoodRepository LikedGoodRepository
//        {
//            get => likedGoodRepository == null
//? new LikedGoodRepository(context) : likedGoodRepository;
//        }

//        public IHistoryElementRepository HistoryElementRepository
//        {
//            get => historyElementRepository == null ?
//new HistoryElementRepository(context) : historyElementRepository;
//        }

//        public IHistoryRepository HistoryRepository
//        {
//            get => historyRepository == null ?
//new HistoryRepository(context) : historyRepository;
//        }

//        public IOrderRepository OrderRepository
//        {
//            get
//            {

//                if (orderRepository == null)
//                {
//                    this.orderRepository = new OrderRepository(context);
//                }
//                return orderRepository;

//            }
//        }
//        public IGoodRepository GoodRepository
//        {
//            get
//            {

//                if (goodRepository == null)
//                {
//                    this.goodRepository = new GoodRepository(context);
//                }
//                return goodRepository;

//            }
//        }
//        public IManufactureRepository ManufacturerRepository
//        {
//            get
//            {
//                if (manufacturerRepository == null)
//                {
//                    this.manufacturerRepository = new ManufacturerRepository(this.context);
//                }
//                return manufacturerRepository;
//            }
//        }

//        #endregion

//        private int amountOfGoodInBasket;
//        public async Task<int> GetAmountOfGoodInBasket()
//        {
//            User user = await userManager.FindByNameAsync(signInManager.Context.User.Identity.Name);
//            var basket = context.Baskets.Where(b => b.IsInOrder == false).
//            FirstOrDefault(b => b.UserId == user.Id);
//            if (basket != null)
//            {
//                return basket.AmountOfGood;
//            }
//            else
//            {
//                return 0;
//            }

//        }
//        public User CurrentUser = new User() { Id = 6, Email = "12", Name = "12", Password = "12", LastName = "12", City = "12" };


//        public void Dispose()
//        {
//            context.Dispose();
//        }


//        public ShopContext GiveContext()
//        {
//            return this.context;
//        }
//        public void Save()
//        {
//            context.SaveChanges();
//        }

//        public async Task SaveAsync()
//        {
//            await context.SaveChangesAsync();
//        }
//    }
//}
