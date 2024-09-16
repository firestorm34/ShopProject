using ShopProject.Data.Interfaces;
using ShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Data
{
    public class ManufacturerRepository: GenericRepository<Manufacturer>, IManufactureRepository
    {
        ShopContext context;
        public ManufacturerRepository(ShopContext context):base(context)
        {
            this.context = context;
        }


    }
}
