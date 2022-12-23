using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp.Service.Interfaces
{
    public interface IBasketService
    {
        Task<IBaseResponse<IEnumerable<OrderViewModel>>> GetItems(string userName);

        Task<IBaseResponse<OrderViewModel>> GetItem(string userName, long id);
    }
}
