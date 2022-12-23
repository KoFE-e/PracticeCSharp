


using Automarket.DAL.Repositories;
using Automarket.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.DAL.Repositories;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Service.Implementations;
using PracticeCSharp.Service.Interfaces;

namespace PracticeCSharp
{
    public static class Initializer
    {
        public static void InitializeRepositories (this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Car>, CarRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Profile>, ProfileRepository>();
            services.AddScoped<IBaseRepository<Order>, OrderRepository>();
            services.AddScoped<IBaseRepository<Basket>, BasketRepository>();
        }

        public static void InitializeServices (this IServiceCollection services)
        {
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();
        }
    }
}
