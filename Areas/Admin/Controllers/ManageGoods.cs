using Microsoft.AspNetCore.Mvc;
using ShopProject.Models;
using ShopProject.Data;
using ShopProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageGoodsController : Controller
    {
        UnitOfWork unit;
        UserManager<User> userManager;
        SignInManager<User> signInManager;
        public ManageGoodsController(UnitOfWork unit, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.unit = unit;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            
           var goods= await unit.GoodRepository.GetAllAsync();
           return View(goods);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Manufacturers = await unit.ManufacturerRepository.GetAllAsync();
            ViewBag.Categories = await unit.CategoryRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Good good)
        {
            await unit.GoodRepository.AddAsync(good);
            await unit.SaveAsync();
            return RedirectToAction("Index", "ManageGoods");
        }



        public async Task<IActionResult> Delete(int good_id)
        {
            var good = await unit.GoodRepository.GetAsync(good_id);
            good.IsArchieved = true;
            await unit.SaveAsync();
            if (good != null)
            {
                await unit.GoodRepository.DeleteAsync(good.Id);
                await unit.SaveAsync();
            }
            return RedirectToAction("Index", "ManageGoods");
        }


        public async Task<IActionResult> Update(int good_id)
        {
            Good good = await unit.GoodRepository.GetAsync(good_id);
           
            ViewBag.Manufacturers = await unit.ManufacturerRepository.GetAllAsync();
            ViewBag.Categories = await unit.CategoryRepository.GetAllAsync();
            return View(good);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Good good)
        {


            unit.GoodRepository.Update(good);
            await unit.SaveAsync();
            return RedirectToAction("Index", "ManageGoods");
        }

    }
}
