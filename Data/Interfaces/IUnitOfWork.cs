using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.Data.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {

        public void Save();

       

    }
}
