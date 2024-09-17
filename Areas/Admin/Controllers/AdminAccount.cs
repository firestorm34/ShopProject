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
    [Route("admin")]
    public class AdminAccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly UnitOfWork unit;
        public AdminAccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            UnitOfWork unit)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unit = unit;
        }
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return View();
            }
            return View("Login",new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await userManager.FindByEmailAsync(model.Email);  
                    if(user is null)
                    {
                        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                        return View(model);
                    }

                    IList<string> roleNames = await userManager.GetRolesAsync(user);
                    if (roleNames.Contains("Admin"))
                    {
                        
                        var result =
                            await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                       
                        
                        if (result.Succeeded)
                        {
                            
                             return RedirectToAction("Index");
                            
                        }

                        else
                        {
                            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                            return View(model);
                        }

                    }

                    ModelState.AddModelError("", "This user doesn't have admin privileges.");
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e.ToString());
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
