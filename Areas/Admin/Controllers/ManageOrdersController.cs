﻿using Microsoft.AspNetCore.Mvc;
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
    public class ManageOrdersController : Controller
    {
        UnitOfWork unit;

        public ManageOrdersController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await unit.OrderRepository.GetAllAsync();

            return View(orders);
        }

        public async Task<IActionResult> Update(int order_id)
        {
            Order order = await unit.OrderRepository.GetAsync(order_id);
            return View(order);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Order order)
        {
            if (ModelState.IsValid)
            {
                var orderedit = await unit.OrderRepository.GetAsync(order.Id);
                orderedit.Status = order.Status;
                await unit.SaveAsync();
                return RedirectToAction("Index");
            }
            
            return View(order);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int order_id)
        {
            await unit.OrderRepository.DeleteAsync(order_id);
            await unit.SaveAsync();
            return RedirectToAction("Index", "Orders");
        }

    }
}
