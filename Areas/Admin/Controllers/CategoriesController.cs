using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
   
    public class CategoriesController:Controller
    {
        public UnitOfWork unit;
        public CategoriesController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var categories = await unit.CategoryRepository.GetAll();
            return View(categories);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await unit.CategoryRepository.GetAllForManage();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await unit.CategoryRepository.Add(category);
            await unit.SaveAsync();
            return RedirectToAction("Index", "Categories");
        }



        public async Task<IActionResult> Delete(int category_id)
        {
            await unit.CategoryRepository.Delete(category_id);
            await unit.SaveAsync();

            return RedirectToAction("Index", "Categories");
        }


        public async Task<IActionResult> Update(int category_id)
        {
            Category category = await unit.CategoryRepository.Get(category_id);
            
            ViewBag.Categories = await unit.CategoryRepository.GetAllForManage();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
         

            unit.CategoryRepository.Update(category);
            await unit.SaveAsync();
            return RedirectToAction("Index", "Categories");
        }

    }
}
