using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Data;
using ShopProject.Models;
namespace ShopProject.Data.Interfaces
{
    public interface IHistoryElementRepository: IGenericRepository<HistoryElement>
    {
        public List<HistoryElement> GetAllByHistoryId(int userid);
    }
}
