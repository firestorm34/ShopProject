using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;


namespace ShopProject.Controllers
{
    public class HistoryController : Controller
    {
       UnitOfWork unit;

        public HistoryController( UnitOfWork unit)
        {
            this.unit = unit;
        }

        public async Task<IActionResult> Index()
        {
            var history = await unit.HistoryRepository.GetByUserId(unit.CurrentUser.Id);
            var historyelements = unit.HistoryElementRepository.GetAllByHistoryId( history.Id);
            List<Good> Goods = new List<Good>();
            ViewBag.IsLiked = new List<bool>();
            foreach(var historyelement in historyelements)
            {

                var good_id = historyelement.ViwedGoodId;
                var good = await unit.GoodRepository.Get(good_id);
                if (good != null)
                {
                    var isliked = await unit.LikedGoodRepository.GetByGoodAndUserId(good.Id, unit.CurrentUser.Id);
                    if (isliked != null)
                    {
                        ViewBag.IsLiked.Add(true);
                    }
                    else
                    {
                        ViewBag.IsLiked.Add(false);
                    }
                    Goods.Add(good);
                }
                if(good== null)
                {
                   await unit.HistoryElementRepository.Delete(historyelement.Id);
                }
            }

            return View(Goods);
        }
    }
}
