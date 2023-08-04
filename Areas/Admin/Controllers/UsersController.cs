using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopProject.Data;
using ShopProject.Models;
using ShopProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private UnitOfWork unit;
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create() => View();

        

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Name = user.Name, LastName = user.LastName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.LastName = model.LastName;
                    user.Name = model.Name;
                    user.City = model.City;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }


        //[HttpGet]
        //public IActionResult Login()
        //{

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var user  = await unit.UserRepository.TryLogin(model);
        //        if (user != null)
        //        {
        //            Authenticate(user);
        //            return RedirectToAction("Index", "Home");

        //        }
        //        ModelState.AddModelError("", "Wrong e-mail or password");
        //    }
        //    return View(model);
        //}


        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var u = await unit.UserRepository.CheckEmail(user.Email);
        //        if (u == null)
        //        {
        //            await unit.UserRepository.Add(user);
        //            unit.Save();
        //            Authenticate(user);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        ModelState.AddModelError("", "User with that e-mail already exists!");
        //    }
        //    return View(user);
        //}

        //public void Authenticate(User user)
        //{
        //    unit.CurrentUser = user;

        //}

        //public IActionResult Logout()
        //{
        //    unit.CurrentUser = null;
        //    return RedirectToAction("Index", "Home");
        //}



    }
}
