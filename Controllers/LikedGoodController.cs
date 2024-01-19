using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data;
namespace ShopProject.Controllers
{
    public class LikedGoodController : Controller
    {
        UnitOfWork unit;

        public LikedGoodController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        public async Task<IActionResult> Index()
        {
            if (unit.CurrentUser is null)
            {
                return View();
            }
            var likedgoods = await unit.LikedGoodRepository.GetAllByUserId(unit.CurrentUser.Id);
           
            if (likedgoods == null)
            {
                return View();
            }
            List<Good> Goods = new List<Good>();
            ViewBag.IsLiked = new List<bool>();
            foreach ( var likedgood in likedgoods.ToList())
            {
                var good = await unit.GoodRepository.GetAsync(likedgood.GoodId);
                ViewBag.IsLiked.Add(true);
                Goods.Add(good);
            }
            return View(Goods);
        }

        public async Task<IActionResult> LikeFromIndexHome(int good_id, string ControllerName, string ActionName)
        {
            await Like(good_id);
            return RedirectToAction(ActionName, ControllerName);
        }

        public async Task<IActionResult> DislikeFromIndexHome(int good_id, string ControllerName, string ActionName)
        {
            await Dislike(good_id);
            return RedirectToAction(ActionName, ControllerName);
        }

        public async Task<IActionResult> LikeFromIndexLikedGood(int good_id)
        {
           await Like(good_id);
            return RedirectToAction("Index", "LikedGood");
        }

        public async Task<IActionResult> DislikeFromIndexLikedGood(int good_id)
        {
           await Dislike(good_id);
            return RedirectToAction("Index", "LikedGood");
        }

        public async Task<IActionResult> LikeFromIndexGood(int good_id)
        {
            await Like(good_id);
            var good = await unit.GoodRepository.GetAsync(good_id);
            return RedirectToAction("Index", "Good", new { category_id = good.CategoryId });
        }

        public async Task<IActionResult> DislikeFromIndexGood(int good_id)
        {
            await Dislike(good_id);
            var good = await unit.GoodRepository.GetAsync(good_id);
            return RedirectToAction("Index", "Good", new { category_id = good.CategoryId });
        }

        public async Task<IActionResult> LikeFromDetailedGood(int good_id)
        {
            await Like(good_id);
            return RedirectToAction("Detailed", "Good", new { good_id = good_id });
        }

        public async Task<IActionResult> DislikeFromDetailedGood(int good_id)
        {
            await Dislike(good_id);
            return RedirectToAction("Detailed", "Good", new { good_id = good_id });
        }

        public async Task<IActionResult> LikeFromIndexHistory(int good_id)
        {
            await Like(good_id);
            return RedirectToAction("Index", "History");
        }

        public async Task<IActionResult> DislikeFromIndexHistory(int good_id)
        {
            await Dislike(good_id);
            return RedirectToAction("Index", "History");
        }

        public async Task Like(int good_id)
        {
            await unit.LikedGoodRepository.AddAsync(new LikedGood { GoodId = good_id, UserId = unit.CurrentUser.Id });
            await unit.SaveAsync();
 
        }
        public async Task Dislike(int good_id)
        {
            var likedgood = await unit.LikedGoodRepository.GetByGoodAndUserId(good_id, unit.CurrentUser.Id);
            if (likedgood != null)
            {
                await unit.LikedGoodRepository.DeleteAsync(likedgood.Id);
                await unit.SaveAsync();
                
            }
           
           
        }

    }
}
