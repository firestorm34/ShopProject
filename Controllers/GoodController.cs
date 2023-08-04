using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data;
using ShopProject.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ShopProject.Controllers
{
    public class GoodController : Controller
    {
        UnitOfWork unit;
        UserManager<User> userManager;
        SignInManager<User> signInManager;
        public GoodController(UnitOfWork unit, UserManager<User> userManager, SignInManager<User> signInManager)
        {
           this.unit = unit;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        



        public async Task<IActionResult> Index(int category_id)
        {
            
            if(category_id == 0)
            {
                return BadRequest("Category 0 doesnt exist!");
            }
            if (unit.CurrentUser != null)
            {
                Category category =  await unit.CategoryRepository.Get(category_id);
                var goods = await unit.GoodRepository.GetAll();
                var categories = unit.CategoryRepository.GetChildCategories(category_id);
                var neededgoods = goods.Where(g => g.Category == category);
                if (categories != null)
                {
                    categories.Add(category);
                    neededgoods = goods.Where(g => categories.Contains(g.Category));

                }

                ViewBag.IsLiked = new List<bool>();

                foreach(var good in neededgoods.ToList())
                {
                    var isliked = await unit.LikedGoodRepository.GetByGoodAndUserId(good.Id, unit.CurrentUser.Id);
                    if(isliked != null)
                    {
                        ViewBag.IsLiked.Add(true);
                    }
                    else
                    {
                        ViewBag.IsLiked.Add(false);
                    }
                    
                }
                return View(neededgoods);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Search(string good_name)
        {
            var neededgoods = unit.GoodRepository.GetByName(good_name);
            if (neededgoods == null)
            {
                ViewBag.Error = "There is not any good with that name";
                return View();
            }
            return View(neededgoods);
        }


        public async Task<IActionResult> Detailed(int good_id)
        {
            var history = await unit.HistoryRepository.GetByUserId(unit.CurrentUser.Id);

            await unit.HistoryElementRepository.Add
                (new HistoryElement { HistoryId = history.Id, ViwedGoodId = good_id });
            unit.Save();

            ViewBag.Available = true;
            if (good_id ==0)
            {
                return RedirectToAction("Index", "Good", new { category_id = 1});
            }
            var goodatstock = await unit.GoodAtStockRepository.GetByGoodId(good_id);

            if (goodatstock == null )
            {
                 goodatstock = await unit.GoodAtStockRepository.
                    Add(new GoodAtStock { GoodId = good_id, AmountLeft = 0 });
                unit.Save();
            }
            if(goodatstock.AmountLeft < 1)
            {
                ViewBag.Available = false;
            }
            var good = await unit.GoodRepository.Get(good_id);
            if(good== null)
            {
                return BadRequest();
            }
            var isliked =await unit.LikedGoodRepository.GetByGoodAndUserId(good_id, unit.CurrentUser.Id);
            if(isliked== null)
            {
                ViewBag.IsLiked = false;
            }
            else
            {
                ViewBag.IsLiked = true;
            }
            return View(good);
        }

        public async Task<IActionResult> Buy(int goodid)
        {
            if (goodid > 0)
            {
               
                var good = await unit.GoodRepository.Get(goodid);
                if (ModelState.IsValid)
                {
                    var a = User;
                    User user = await userManager.FindByNameAsync(User.Identity.Name);
                    var basket = await unit.BasketRepository.GetByUserId(user.Id);
                    if (basket == null)
                    {
                        
                        basket = await unit.BasketRepository.Add(new Basket {
                            UserId =user.Id});
                    }
                    await unit.SaveAsync();
                    var goodinbasket = await unit.GoodInBasketRepository.GetByGoodAndBasketId(good.Id,(int)basket.Id);
                    if (goodinbasket == null)
                    {
                        goodinbasket = await unit.GoodInBasketRepository.Add(new GoodInBasket { GoodId = good.Id, BasketId = basket.Id });
                        basket.AmountOfGood++;
                        
                    }

                    goodinbasket.Amount++;
                    goodinbasket.SumOfGoods = (good.Price * (decimal)goodinbasket.Amount);
                    basket.TotalSum += good.Price;


                    await unit.SaveAsync();
                    return RedirectToAction("Detailed", "Good", new { good_id = good.Id });
                    //HERE SHOULD BE CODE TO MARK BUYING GOOD!

                }
                return RedirectToAction("Detailed", "Good", new { good_id = good.Id });
            }
            ModelState.AddModelError("", "Current good doesnt exist");
            return RedirectToAction("Index", "Good", new { category_id =  1});

        }








    }
}
