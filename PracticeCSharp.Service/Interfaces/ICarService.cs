using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Car;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PracticeCSharp.Service.Interfaces
{
    public interface ICarService
    {
        IBaseResponse<List<Car>> GetCars();

        Task<IBaseResponse<CarViewModel>> GetCar(long id);

        Task<BaseResponse<Dictionary<long, string>>> GetCar(string term);

        Task<IBaseResponse<Car>> Create(CarViewModel carViewModel);

        Task<IBaseResponse<bool>> DeleteCar(long id);

        Task<IBaseResponse<Car>> Edit(long id, CarViewModel model);
        BaseResponse<Dictionary<long, string>> GetTypes();
    }
}
