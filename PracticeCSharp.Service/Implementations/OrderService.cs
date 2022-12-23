using Microsoft.EntityFrameworkCore;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Enum;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Order;
using PracticeCSharp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp.Service.Implementations
{
    public class OrderService : IOrderService
    {

        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Car> _carRepository;

        public OrderService(IBaseRepository<User> userRepository, IBaseRepository<Order> orderRepository, IBaseRepository<Car> carRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _carRepository = carRepository;
        }

        public async Task<IBaseResponse<Order>> Create(CreateOrderViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x => x.Basket)
                    .FirstOrDefaultAsync(x => x.Name == model.Login);
                if (user == null)
                {
                    return new BaseResponse<Order>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var order = new Order()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Address = model.Address,
                    DateCreated = DateTime.Now,
                    BasketId = user.Basket.Id,
                    CarId = model.CarId,
                    Avatar = model.Avatar
                };

                await _orderRepository.Create(order);
                return new BaseResponse<Order>()
                {
                    Description = "Заказ создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(long id)
        {
            try
            {
                var order = _orderRepository.GetAll()
                .Select(x => x.Basket.Orders.FirstOrDefault(y => y.Id == id))
                .FirstOrDefault();
                if (order == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.OrderNotFound,
                        Description = "Заказ не найден"
                    };
                }

                await _orderRepository.Delete(order);
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = "Заказ удален"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
            

        }
    }
}
