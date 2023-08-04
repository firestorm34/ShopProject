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
            Basket basket =await unit.BasketRepository.GetByUserId(user_id);
            User user = await unit.UserRepository.Get(user_id);
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
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Make(Order order)
        {
            order.User = null;
            order.Basket = null;
            order.Status = Status.InProcess;
            order.User = await unit.UserRepository.Get(unit.CurrentUser.Id);
            order.Basket = await unit.BasketRepository.GetByUserId(order.User.Id);
           await unit.OrderRepository.Add(order);
            var goodsinbasket = unit.GoodInBasketRepository.GetAllByBasketId((int)order.Basket.Id);

            List<GoodAtStock> StockGoods = new List<GoodAtStock>();
            foreach (var goodinbasket in goodsinbasket.ToList())
            {
                var goodatstock = await unit.GoodAtStockRepository.GetByGoodId(goodinbasket.GoodId);
                if (goodatstock.AmountLeft < goodinbasket.Amount)
                {
                    ModelState.AddModelError("", "Unfortunately, this good is not available!");
                    return RedirectToAction("Index", "Basket", unit.CurrentUser.Id);
                }
                StockGoods.Add(goodatstock);
            }
            int i = 0;
            foreach (var goodinbasket in goodsinbasket.ToList())
            {
                var goodatstock = StockGoods[i];
                goodatstock.AmountLeft -= goodinbasket.Amount;
               

                i++;
            }
            order.Basket.IsInOrder = true;
            unit.Save();
            //unit.AmountOfGoodInBasket = 0;
            return RedirectToAction("Index", "Home");

        }

    }
}
