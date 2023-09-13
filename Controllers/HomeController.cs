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
        
        public HomeController(ILogger<HomeController> logger, UnitOfWork unit)
        {
            this.logger = logger;
            this.unit = unit;
           
        }

        public async Task<IActionResult> Index()
        {
            
            List<Good> goods = await unit.GoodRepository.GetAllAsync();
            List<Category> start_categories = unit.CategoryRepository.GetByParentCategory(0);
            List<Good> neededgoods = goods.Where(g => g.CategoryId == 0).ToList();
            List<Category> child_categories = new List<Category>();
            if (start_categories != null)
            {
                
                foreach (Category cat in start_categories) {
                    List<Category> child_cat = unit.CategoryRepository.GetChildCategories(cat.Id);
                    if (child_cat != null) { 
                        child_categories.Concat(child_cat);
                    }
                }
                IEnumerable<Category> all_child_categories = start_categories.Concat(child_categories);

                neededgoods = goods.Where(g => all_child_categories.Contains(g.Category)).ToList();
                int j = 1;
            }
            if (neededgoods.Count() == 0)
            {
                return View();
            }
            ViewBag.Likes = await GetLikesForGoods(neededgoods);
            return View(neededgoods);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<dynamic> GetLikesForGoods(IEnumerable<Good> neededgoods)
        {
            dynamic Likes = new ExpandoObject();
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
