using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using Microsoft.AspNetCore.Identity;
using ShopProject.ViewModels;
using System.Security.Claims;
using Serilog;
using Serilog.Data;
using Serilog.Core;
using ShopProject.Data;

namespace ShopProject.Controllers
{
    public class AccountController : Controller
    {
        string Changes = "";
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private ILogger logger;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger logger)
            {
                this.userManager = userManager;
               this.signInManager = signInManager;
                this.logger = logger;
            
            }
       

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                EditUserViewModel model = new();
                User user = await userManager.FindByNameAsync(User.Identity.Name);
                model.Name = user.Name;
                model.LastName = user.LastName;
                model.City = user.City;
                model.Id = user.Id;
                model.Email = user.Email;
                if (Changes != "") { ViewBag.Alert = Changes; }
                return View("Index",model);
            }
            logger.Warning("User is not authenticated");
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            EditUserViewModel model = new();
            User user = await userManager.FindByNameAsync(User.Identity.Name);
            model.Name = user.Name;
            model.LastName = user.LastName;
            model.City = user.City;
            model.Id = user.Id;
            model.Email = user.Email;
            return View("Edit",model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id.ToString());
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.LastName = model.LastName;
                    user.Name = model.Name;
                    user.City = model.City;
                    if(model.IsPasswordChanged == true)
                    {
                        IdentityResult pwd_change =
                        await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                            if (!pwd_change.Succeeded)
                            {
                                ModelState.AddModelError("", "Old password is incorrect!");
                                return View("Edit",model);
                            }
                            ViewBag.Changes = "<p> Password was successfully changed </p>";
                    }

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        ViewBag.Changes += " <p> All changed was saved </p>";
                        return RedirectToAction("Index", "Account") ;
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("a", error.Description);
                        }
                        return View("Edit", model);
                    }
                }
                ModelState.AddModelError("", "Error! There is no such user!");
            }
            logger.Warning("{controller}/{action}/{method} : Model is not valid! ",
                "AccountControler", "Edit", "Post");
            return View("Edit",model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email};

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("Register",model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {              
                    return RedirectToAction("Index", "Home");
                    
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View("Login",model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
