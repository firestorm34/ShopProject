using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopProject.Data;
using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UnitOfWork _unit;

        public HomeController(ILogger<HomeController> logger, UnitOfWork unit)
        {
            _logger = logger;
            _unit = unit;
        }

        public async Task<IActionResult> Index()
        {
            
            List<Good> goods = await _unit.GoodRepository.GetAllAsync();
            List<Category> start_categories = _unit.CategoryRepository.GetByParentCategory(0);
            List<Good> neededgoods = goods.Where(g => g.CategoryId == 0).ToList();
            List<Category> child_categories = new List<Category>();
            if (start_categories != null)
            {
                
                foreach (Category cat in start_categories) {
                    List<Category> child_cat = _unit.CategoryRepository.GetChildCategories(cat.Id);
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
    }
}
