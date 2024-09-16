using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopProject.Data;
using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UnitOfWork unit;
        public dynamic Likes = new ExpandoObject();
        public HomeController(ILogger<HomeController> logger, UnitOfWork unit)
        {
            this.logger = logger;
            this.unit = unit;
           
        }

        public async Task<IActionResult> Index(string ErrorMessage=null)
        {
            
            List<Good> goods = await unit.GoodRepository.GetAllAsync();
            List<Category> main_categories = unit.CategoryRepository.GetByParentCategory(0);
            List<Good> neededgoods = goods.Where(g => g.CategoryId == 0).ToList();
            List<Category> child_categories = new List<Category>();
            if (main_categories != null) 
            {
                
                foreach (Category cat in main_categories) {
                    List<Category> child_cat = unit.CategoryRepository.GetChildCategories(cat.Id);
                    if (child_cat != null) { 
                        child_categories.Concat(child_cat);
                    }
                }
                IEnumerable<Category> all_child_categories = main_categories.Concat(child_categories);

                neededgoods = goods.Where(g => all_child_categories.Contains(g.Category)).ToList();
            }
            if (neededgoods.Count() == 0)
            {
                return View();
            }
            ViewBag.Likes = await GetLikesForGoods(neededgoods);
            if (ErrorMessage is not null)
            {
                ViewBag.ErrorMessage = ErrorMessage;
            }
            return View(neededgoods);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<dynamic> GetLikesForGoods(IEnumerable<Good> neededgoods)
        {
             Likes = new ExpandoObject();
            if (neededgoods==null || neededgoods.ToList().Count == 0)
            {
                Likes.ShowLike = false;
                return Likes;
            }
            
            Likes.ShowLike = true;
            Likes.IsLiked = new Dictionary<int,bool>();
            if (unit.CurrentUser != null)
            {

                foreach (var good in neededgoods.ToList())
                {
                    LikedGood likedGood = await unit.LikedGoodRepository.GetByGoodAndUserId(good.Id, unit.CurrentUser.Id);
                    if (likedGood != null)
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
            
            var a = Likes.IsLiked;
            return Likes;
        }

    }
}
