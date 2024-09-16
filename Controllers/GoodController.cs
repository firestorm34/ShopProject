using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data;
using ShopProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Dynamic;
using Microsoft.Extensions.DependencyInjection;

namespace ShopProject.Controllers
{
    [Controller]
    public class GoodController : Controller
    {
        UnitOfWork unit;
        UserManager<User> userManager;
        IServiceScopeFactory scopeFactory;
        public GoodController(UnitOfWork unit, UserManager<User> userManager)
        {
            this.unit = unit;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int category_id)
        {
            if(category_id == 0)
            {
                return BadRequest("Category 0 doesnt exist!");
            }
            IEnumerable<Good> neededgoods;
            Category category =  await unit.CategoryRepository.GetAsync(category_id);
            List<Good> goods = await unit.GoodRepository.GetAllAsync();
            List<Category> childCategories = unit.CategoryRepository.GetChildCategories(category_id);

            if (childCategories != null)
            {
                childCategories.Add(category);
                neededgoods = goods.Where(g => childCategories.Contains(g.Category));
            }
            else
            {
                neededgoods = goods.Where(g => g.Category == category);
            }
            ViewBag.Likes = await GetLikesForGoods(neededgoods);
            return View(neededgoods.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm]string Search_text)
        {
            List<Good> neededgoods = await unit.GoodRepository.GetByNameAsync(Search_text);
            if (neededgoods == null || neededgoods.Count == 0)
            {
                ViewBag.Error = "There is not any good with that name";
                return View("Index");
            }
            ViewBag.Likes = await GetLikesForGoods(neededgoods);
            return View("Index",neededgoods);
        }


        public async Task<IActionResult> Detailed(int good_id)
        {
            if (good_id == 0)
            {
                return RedirectToAction("Index", "Good", new { category_id = 1 });
            }

            var good = await unit.GoodRepository.GetAsync(good_id);
            if (good == null)
            {
                return BadRequest();
            }

            if (unit.CurrentUser is not  null) {
                var history = await unit.HistoryRepository.GetByUserId(unit.CurrentUser.Id);

                await unit.HistoryElementRepository.AddAsync
                    (new HistoryElement { HistoryId = history.Id, ViwedGoodId = good_id });
                unit.Save();
                ViewBag.Likes = await GetLikesForGoods(new []{good});
            }

            ViewBag.Available = true;
           
            var goodatstock = await unit.GoodAtStockRepository.GetByGoodId(good_id);

            if (goodatstock == null )
            {
                 goodatstock = await unit.GoodAtStockRepository.
                    AddAsync(new GoodAtStock { GoodId = good_id, AmountLeft = 0 });
                unit.Save();
            }
            if(goodatstock.AmountLeft < 1)
            {
                ViewBag.Available = false;
            }

            return View(good);
        }


        public async Task<IActionResult> Buy(int goodid)
        {
            if (goodid > 0)
            {
               
                var good = await unit.GoodRepository.GetAsync(goodid);
                if (ModelState.IsValid)
                {
                    var a = User;
                    User user = await userManager.FindByNameAsync(User.Identity.Name);
                    var basket = await unit.BasketRepository.GetByUserId(user.Id);
                    if (basket == null)
                    {
                        
                        basket = await unit.BasketRepository.AddAsync(new Basket {
                            UserId =user.Id});
                    }
                    await unit.SaveAsync();
                    var goodinbasket = await unit.GoodInBasketRepository.GetByGoodAndBasketId(good.Id,(int)basket.Id);
                    if (goodinbasket == null)
                    {
                        goodinbasket = await unit.GoodInBasketRepository.AddAsync(new GoodInBasket { GoodId = good.Id, BasketId = basket.Id });
                        basket.AmountOfGood++;
                        
                    }

                    goodinbasket.Amount++;
                    goodinbasket.SumOfGoods = (good.Price * (decimal)goodinbasket.Amount);
                    basket.TotalSum += good.Price;


                    await unit.SaveAsync();
                    return RedirectToAction("Detailed", "Good", new { good_id = good.Id });
                   

                }
                return RedirectToAction("Detailed", "Good", new { good_id = good.Id });
            }
            ModelState.AddModelError("", "Current good doesnt exist");
            return RedirectToAction("Index", "Good", new { category_id =  1});

        }


        public async Task<dynamic> GetLikesForGoods( IEnumerable<Good> neededgoods)
        {
            dynamic Likes = new ExpandoObject();
            Likes.ShowLike = true;
            Likes.IsLiked = new Dictionary<int,bool>();

            if (unit.CurrentUser != null)
            {

                foreach (var good in neededgoods.ToList())
                {
                    var isliked = await unit.LikedGoodRepository.GetByGoodAndUserId(good.Id, unit.CurrentUser.Id);
                    if (isliked != null)
                    {
                        Likes.IsLiked.Add(good.Id,true);
                    }
                    else
                    {
                        Likes.IsLiked.Add(good.Id,false);
                    }

                }
            }
            else
            {

                Likes.ShowLike = false;
            }
            return Likes;
        }





    }
}
