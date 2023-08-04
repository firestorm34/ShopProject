using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopProject.Models;
using ShopProject.Data.Interfaces;
namespace ShopProject.Data
{
    public class HistoryElementRepository: GenericRepository<HistoryElement>, IHistoryElementRepository
    {
        ShopContext context;
        public HistoryElementRepository(ShopContext context):base( context)
        {
            this.context = context;
        }

        public  List<HistoryElement> GetAllByHistoryId(int historyid)
        {
            return context.HistoryElements.Where(h => h.HistoryId == historyid).ToList();
        }


        public async new Task<HistoryElement> Add(HistoryElement element)
        {
            var findel = context.HistoryElements.FirstOrDefault(h => h.HistoryId == element.HistoryId &&
            h.ViwedGoodId == element.ViwedGoodId);
            var history = await context.Histories.FindAsync(element.HistoryId);
            if (findel == null)
            {


                if (history.AmountOfViwedGoods == 2)
                {
                    var elements = context.HistoryElements.Where(h => h.HistoryId == element.HistoryId).ToList();
                    if(elements != null)
                    {
                        var e = elements.OrderBy(E => E.Id).FirstOrDefault();
                        if (e != null)
                        {
                            context.HistoryElements.Remove(e);
                        }
                    }
                   
                }
                if (history.AmountOfViwedGoods > 2)
                {   while (history.AmountOfViwedGoods > 2)
                    {
                        var elements = context.HistoryElements.ToList();
                        if (elements != null)
                        {
                            var e = elements.OrderBy(E => E.Id).FirstOrDefault();
                            if (e != null)
                            {
                                context.HistoryElements.Remove(e);
                            }
                        }
                    }
                }
             
                await context.HistoryElements.AddAsync(element);
                if (history.AmountOfViwedGoods < 2)
                {
                    history.AmountOfViwedGoods++;
                }
            }
            

            return element;
        }
    }
}
