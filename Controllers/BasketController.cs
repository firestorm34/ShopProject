using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
using NuGet.ContentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ShopProject.Controllers
{
    public class BasketController : Controller
    {
        
        UnitOfWork unit;
        IServiceScopeFactory scopeFactory;

        //public BasketController(UnitOfWork unit)
        //{
        //    this.unit = unit;

        //}
        public BasketController(UnitOfWork unit, IServiceScopeFactory factory)
        {
            this.unit = unit;
            this.scopeFactory = factory;

        }

        public async Task<IActionResult> Index()
        {
            
            Basket basket = await unit.BasketRepository.GetByUserId(unit.CurrentUser.Id);
            if (basket == null)
            {
                basket = await unit.BasketRepository.AddAsync(new Basket
                {
                    UserId = unit.CurrentUser.Id,
                });
            }
            BasketViewModel model = await GetBasketViewModelAsync(basket);
            return View(model);
        }

        public async Task<IActionResult> Delete(int goodid)
        {
            var basket = await unit.BasketRepository.GetByUserId(unit.CurrentUser.Id);
            var goodinbasket = await unit.GoodInBasketRepository.GetByGoodAndBasketId(goodid,(int)basket.Id);
            await unit.GoodInBasketRepository.DeleteAsync(goodinbasket.Id);
            
            basket.AmountOfGood--;
            basket.TotalSum -= (decimal)goodinbasket.Amount * goodinbasket.Good.Price;
            unit.Save();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Show(int basket_id)
        {
            var basket = await unit.BasketRepository.GetByIdFromOrder(basket_id);
            if(basket != null){
                BasketViewModel model = await GetBasketViewModelAsync(basket);
                return View(model);
            }
            return BadRequest("Basket with that id doesn't exist");
        }

       public async Task<BasketViewModel> GetBasketViewModelAsync(Basket basket)
        {
            BasketViewModel model = new BasketViewModel();
            model.CanBuy = true;
         
            var goodsinbasket = unit.GoodInBasketRepository.GetAllByBasketId(basket.Id);
            model.TotalSum = basket.TotalSum;
            model.UserId = unit.CurrentUser.Id;

            foreach (var goodinbasket in goodsinbasket.ToList())
            {
                model.GoodId.Add(goodinbasket.GoodId);
                model.GoodNames.Add(goodinbasket.Good.Name);
                model.GoodPrices.Add(goodinbasket.Good.Price);
                model.GoodAmounts.Add(goodinbasket.Amount);
                model.SumOfGoods.Add(goodinbasket.SumOfGoods);
                var goodatbasket = await unit.GoodAtStockRepository.GetByGoodId(goodinbasket.GoodId);
                if (goodatbasket.AmountLeft < goodinbasket.Amount)
                {
                    model.IsAvailable.Add(false);
                    model.CanBuy = false;
                }
                else
                {
                    model.IsAvailable.Add(true);
                }


            }
            model.AmountOfGoods = basket.AmountOfGood;
            return model;

        }

        public async Task RecalculateTotalSum(int basketid)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                UnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
                Basket basket = await unitOfWork.BasketRepository.GetByUserId(unitOfWork.CurrentUser.Id);
                var goodsinbasket = unitOfWork.GoodInBasketRepository.GetAllByBasketId(basketid);
                decimal totalSum = 0m;
                foreach (var goodinbasket in goodsinbasket.ToList())
                {
                    goodinbasket.SumOfGoods = (goodinbasket.Good.Price * (decimal)goodinbasket.Amount);
                    totalSum += goodinbasket.SumOfGoods;
                }
                basket.TotalSum = totalSum;
                unitOfWork.BasketRepository.Update(basket);
                await unitOfWork.SaveAsync();
            }
        }
        
        public async Task<IActionResult> Plus(int goodid)
        {
            Basket basket = await unit.BasketRepository.GetByUserId(unit.CurrentUser.Id);
            if (basket != null)
            {
                unit.GoodInBasketRepository.IncreaseGoodAmount(goodid, basket.Id);
                unit.Save();
                await RecalculateTotalSum(basket.Id);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Minus(int goodid)
        {
            Basket basket = await unit.BasketRepository.GetByUserId(unit.CurrentUser.Id);
            if (basket != null)
            {
                unit.GoodInBasketRepository.DecreaseGoodAmount(goodid,basket.Id);
                unit.Save();
                await RecalculateTotalSum(basket.Id);
            }

            return RedirectToAction("Index");
        }
    }
}
