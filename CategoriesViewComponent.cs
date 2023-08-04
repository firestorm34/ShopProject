using Microsoft.AspNetCore.Mvc;
using ShopProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject
{
    public class CategoriesViewComponent : ViewComponent
    {
        UnitOfWork unit;
        public CategoriesViewComponent(UnitOfWork unit)
        {
            this.unit = unit;
        }

        public string Invoke()
        {
            return $"Текущее время: {DateTime.Now.ToString("hh:mm:ss")}";
        }
    }
}
