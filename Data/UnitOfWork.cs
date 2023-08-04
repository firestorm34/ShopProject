using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShopProject.Data.Interfaces;
using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShopContext context;
        private SignInManager<User> signInManager;
        private UserManager<User> userManager;
        IServiceScopeFactory serviceScopeFactory;
        public User? CurrentUser {
            get { return context.Users.FirstOrDefault(u => u.UserName == signInManager.Context.User.Identity.Name); }
        }

        public UnitOfWork(ShopContext _context, SignInManager<User> _signInManager, UserManager<User> _userManager,
             IServiceScopeFactory serviceScopeFactory)
        {
            context = _context;
            signInManager = _signInManager;
            userManager = _userManager;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        #region Variables for repositories
        IGoodRepository goodRepository;
        IManufactureRepository manufacturerRepository;
        IOrderRepository orderRepository;
        IUserRepository userRepository;
        IGoodInBasketRepository goodInBasketRepository;
        IBasketRepository basketRepository;
        IAdminRepository adminRepository;
        ICategoryRepository categoryRepository;
        IGoodAtStockRepository goodAtStockRepository;
        IHistoryRepository historyRepository;
        IHistoryElementRepository historyElementRepository;
        ILikedGoodRepository likedGoodRepository;

        #endregion

       

        #region Properties for repositories
        public IUserRepository UserRepository { get => userRepository == null ? new UserRepository(context) : userRepository; }
        public IGoodInBasketRepository GoodInBasketRepository { get => goodInBasketRepository == null ?
                new GoodInBasketRepository(context) : goodInBasketRepository; }
        public IBasketRepository BasketRepository { get => basketRepository == null ? new BasketRepository(context) : basketRepository; }
        public IAdminRepository AdminRepository { get => adminRepository == null ? new AdminRepository(context) : adminRepository; }
        public ICategoryRepository CategoryRepository { get => categoryRepository == null ? new CategoryRepository(context) : categoryRepository; }
        public IGoodAtStockRepository GoodAtStockRepository { get => goodAtStockRepository == null ?
              new GoodAtStockRepository(context) : goodAtStockRepository;
        }
        
        public ILikedGoodRepository LikedGoodRepository { get => likedGoodRepository == null 
                ? new LikedGoodRepository(context) : likedGoodRepository; }

        public IHistoryElementRepository HistoryElementRepository { get => historyElementRepository == null ?
        new HistoryElementRepository(context) : historyElementRepository;
        }

        public IHistoryRepository HistoryRepository { get => historyRepository == null ?
         new HistoryRepository(context) : historyRepository;
        }

        public IOrderRepository OrderRepository
        {
            get
            {

                if (orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(context);
                }
                return orderRepository;

            }
        }
        public IGoodRepository GoodRepository {
            get {

                if (goodRepository == null)
                {
                    this.goodRepository = new GoodRepository(context);
                }
                return goodRepository;

            }
        }
        public IManufactureRepository ManufacturerRepository
        {
            get
            {
                if (manufacturerRepository == null)
                {
                    this.manufacturerRepository = new ManufacturerRepository(this.context);
                }
                return manufacturerRepository;
            }
        }

#endregion


       
        private int amountOfGoodInBasket;
        public  int GetAmountOfGoodInBasket()
        {
            if (signInManager.Context.User.Identity.IsAuthenticated)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetRequiredService<ShopContext>();



                    User user = ctx.Users.FirstOrDefault(u => u.UserName == signInManager.Context.User.Identity.Name);
                    var basket = ctx.Baskets.Where(b => b.IsInOrder == false).
                    FirstOrDefault(b => b.UserId == user.Id);

                    if (basket != null)
                    {
                        return basket.AmountOfGood;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }


        public int GetAmountOfLikedGoods()
        {
            if (signInManager.Context.User.Identity.IsAuthenticated)
            {
                User user = context.Users.FirstOrDefault(u => u.UserName == signInManager.Context.User.Identity.Name);
                var liked_goods = context.LikedGoods.Where(l => l.UserId == user.Id);
                if (liked_goods != null)
                {
                    return liked_goods.ToList().Count;
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }
        public void Dispose()
        {
            context.Dispose();
        }


        public ShopContext GiveContext()
        {
            return this.context;
        }
        public void Save()
        {
             context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
