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
        public virtual User CurrentUser {
            get { return context.Users.FirstOrDefault(u => u.UserName == signInManager.Context.User.Identity.Name); }
        }
        public UnitOfWork() {}
        public UnitOfWork(ShopContext context, SignInManager<User> signInManager)
        {
            this.context = context;
            this.signInManager = signInManager;
        }

        #region Variables for Repositories Interfaces
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
        public virtual IUserRepository UserRepository { get => userRepository == null ? new UserRepository(context) : userRepository; }
        public virtual IGoodInBasketRepository GoodInBasketRepository { get => goodInBasketRepository == null ?
                new GoodInBasketRepository(context) : goodInBasketRepository; }
        public virtual IBasketRepository BasketRepository { get => basketRepository == null ? new BasketRepository(context) : basketRepository; }
        public virtual IAdminRepository AdminRepository { get => adminRepository == null ? new AdminRepository(context) : adminRepository; }
        public virtual ICategoryRepository CategoryRepository { get => categoryRepository == null ? new CategoryRepository(context) : categoryRepository; }
        public virtual IGoodAtStockRepository GoodAtStockRepository { get => goodAtStockRepository == null ?
              new GoodAtStockRepository(context) : goodAtStockRepository;
        }
        
        public virtual ILikedGoodRepository LikedGoodRepository { get => likedGoodRepository == null 
                ? new LikedGoodRepository(context) : likedGoodRepository; }

        public virtual IHistoryElementRepository HistoryElementRepository { get => historyElementRepository == null ?
        new HistoryElementRepository(context) : historyElementRepository;
        }

        public virtual IHistoryRepository HistoryRepository { get => historyRepository == null ?
         new HistoryRepository(context) : historyRepository;
        }

        public virtual IOrderRepository OrderRepository
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
        public virtual IGoodRepository GoodRepository {
            get {

                if (goodRepository == null)
                {
                    this.goodRepository = new GoodRepository(context);
                }
                return goodRepository;

            }
        }
        public virtual IManufactureRepository ManufacturerRepository
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


                    User user = context.Users.FirstOrDefault(u => u.UserName == signInManager.Context.User.Identity.Name);
                    var basket = context.Baskets.Where(b => b.IsInOrder == false).
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

        public virtual void Save()
        {
             context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
