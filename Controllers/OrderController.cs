using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Controllers
{
    public class OrderController : Controller
    {
        UnitOfWork unit;

        public OrderController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        public async Task<IActionResult> Index(int user_id)
        {
            User user = await unit.UserRepository.GetById(user_id);
            if (user is not null)
            {
                Basket basket = await unit.BasketRepository.GetByUserId(user_id);

                if (basket != null)
                {
                    var order = new Order
                    {
                        BasketId = basket.Id,
                        UserId = user.Id,
                        Cost = basket.TotalSum,
                        User = user,
                        Basket = basket,
                        Status = Status.InProcess
                    };
                    return View(order);
                }
                ModelState.AddModelError("", "Server Error! Basket doesn't exists!");
                return View();
            }
            ModelState.AddModelError("", "You are not authenticated!");
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Make(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            order.User = null;
            order.Basket = null; 
            order.Status = Status.InProcess;

            if(unit.CurrentUser == null)
            {
                ModelState.AddModelError("", "Server error! Can't get authenticated user");
                return View("Index");
            }
            order.User = await unit.UserRepository.GetAsync(unit.CurrentUser.Id);


            order.Basket = await unit.BasketRepository.GetByUserId(order.User.Id);
            if(order.Basket == null)
            {
                ModelState.AddModelError("", "Server error! Can't find basket");
                return View("Index");
            }

            var goodsinbasket = unit.GoodInBasketRepository.GetAllByBasketId((int)order.Basket.Id);
            List<GoodAtStock> StockGoods = new List<GoodAtStock>();
            foreach (var goodinbasket in goodsinbasket.ToList())
            {
                var goodatstock = await unit.GoodAtStockRepository.GetByGoodId(goodinbasket.GoodId);
                if (goodatstock.AmountLeft < goodinbasket.Amount)
                {
                    //Доп. проверка, что за время нахождения на странице корзины товар не стал меньше нужного кол-ва
                    // т.е. другой юзер купил весь товар и теперь нельзя сделать заказ
                    ModelState.AddModelError("", "Unfortunately, this good is not available!");
                    return RedirectToAction("Index", "Basket", unit.CurrentUser.Id);
                }
                StockGoods.Add(goodatstock);
            }

            await unit.OrderRepository.AddAsync(order);

            int stockIndex = 0;
            foreach (var goodinbasket in goodsinbasket.ToList())
            {
                var goodatstock = StockGoods[stockIndex];
                goodatstock.AmountLeft -= goodinbasket.Amount;
                stockIndex++;
            }

            order.Basket.IsInOrder = true;
       
            unit.Save();
            return RedirectToAction("Index", "Home");

        }

    }
}
