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

namespace ShopProject.Controllers
{
    public class GoodController : Controller
    {
        UnitOfWork _unit;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        public GoodController(UnitOfWork unit, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _unit = unit;
            _userManager = userManager;
            _signInManager = signInManager;
          
        }



       

        public async Task<IActionResult> Index(int category_id)
        {
            
            if(category_id == 0)
            {
                return BadRequest("Category 0 doesnt exist!");
            }
           
            Category category =  await _unit.CategoryRepository.GetAsync(category_id);
                List<Good> goods = await _unit.GoodRepository.GetAllAsync();
                List<Category> categories = _unit.CategoryRepository.GetChildCategories(category_id);
                IEnumerable<Good> neededgoods = goods.Where(g => g.Category == category);
                if (categories != null)
                {
                    categories.Add(category);
                    neededgoods = goods.Where(g => categories.Contains(g.Category));

                }

                ViewBag.IsLiked = new List<bool>();
                if (_unit.CurrentUser != null)
                {
             
                    foreach (var good in neededgoods.ToList())
                        {
                            var isliked = await _unit.LikedGoodRepository.GetByGoodAndUserId(good.Id, _unit.CurrentUser.Id);
                            if(isliked != null)
                            {
                                ViewBag.IsLiked.Add(true);
                            }
                            else
                            {
                                ViewBag.IsLiked.Add(false);
                            }

                        }
                }
                else
                {

                ViewBag.ShowLike = false;
                }
            return View(neededgoods);
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm]string Search_text)
        {
            List<Good> neededgoods = await _unit.GoodRepository.GetByNameAsync(Search_text);
            if (neededgoods == null || neededgoods.Count == 0)
            {
                ViewBag.Error = "There is not any good with that name";
                return View("Index");
            }
            return View("Index",neededgoods);
        }


        public async Task<IActionResult> Detailed(int good_id)
        {
            if (good_id == 0)
            {
                return RedirectToAction("Index", "Good", new { category_id = 1 });
            }

            var good = await _unit.GoodRepository.GetAsync(good_id);
            if (good == null)
            {
                return BadRequest();
            }

            if (_unit.CurrentUser is not  null) {
                var history = await _unit.HistoryRepository.GetByUserId(_unit.CurrentUser.Id);

                await _unit.HistoryElementRepository.AddAsync
                    (new HistoryElement { HistoryId = history.Id, ViwedGoodId = good_id });
                _unit.Save();
                var isliked = await _unit.LikedGoodRepository.GetByGoodAndUserId(good_id, _unit.CurrentUser.Id);
                if (isliked == null)
                {
                    ViewBag.IsLiked = false;
                }
                else
                {
                    ViewBag.IsLiked = true;
                }
            }
            else
            {
                ViewBag.ShowLike = false;
            }

            ViewBag.Available = true;
           
            var goodatstock = await _unit.GoodAtStockRepository.GetByGoodId(good_id);

            if (goodatstock == null )
            {
                 goodatstock = await _unit.GoodAtStockRepository.
                    AddAsync(new GoodAtStock { GoodId = good_id, AmountLeft = 0 });
                _unit.Save();
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
               
                var good = await _unit.GoodRepository.GetAsync(goodid);
                if (ModelState.IsValid)
                {
                    var a = User;
                    User user = await _userManager.FindByNameAsync(User.Identity.Name);
                    var basket = await _unit.BasketRepository.GetByUserId(user.Id);
                    if (basket == null)
                    {
                        
                        basket = await _unit.BasketRepository.AddAsync(new Basket {
                            UserId =user.Id});
                    }
                    await _unit.SaveAsync();
                    var goodinbasket = await _unit.GoodInBasketRepository.GetByGoodAndBasketId(good.Id,(int)basket.Id);
                    if (goodinbasket == null)
                    {
                        goodinbasket = await _unit.GoodInBasketRepository.AddAsync(new GoodInBasket { GoodId = good.Id, BasketId = basket.Id });
                        basket.AmountOfGood++;
                        
                    }

                    goodinbasket.Amount++;
                    goodinbasket.SumOfGoods = (good.Price * (decimal)goodinbasket.Amount);
                    basket.TotalSum += good.Price;


                    await _unit.SaveAsync();
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
