using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Data;
using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManufacturersController : Controller
    {
        public UnitOfWork unit;
        public ManufacturersController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var manufacturers = await unit.ManufacturerRepository.GetAll();
            return View(manufacturers);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Manufacturer manufacturer)
        {
            await unit.ManufacturerRepository.Add(manufacturer);
            await unit.SaveAsync();
            return RedirectToAction("Index", "Manufacturers");
        }



        public async Task<IActionResult> Delete(int manufacturer_id)
        {
            await unit.ManufacturerRepository.Delete(manufacturer_id);
            await unit.SaveAsync();

            return RedirectToAction("Index", "Manufacturers");
        }


        public async Task<IActionResult> Update(int manufacturer_id)
        {
            Manufacturer manufacturer= await unit.ManufacturerRepository.Get(manufacturer_id);
            return View(manufacturer);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Manufacturer manufacturer)
        {


            unit.ManufacturerRepository.Update(manufacturer);
            await unit.SaveAsync();
            return RedirectToAction("Index", "Manufacturers");
        }

    }
}
