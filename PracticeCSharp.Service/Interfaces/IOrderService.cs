using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IBaseResponse<Order>> Create(CreateOrderViewModel model);

        Task<IBaseResponse<bool>> Delete(long id);

    }
}
