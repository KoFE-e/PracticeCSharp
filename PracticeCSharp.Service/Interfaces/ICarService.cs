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

        Task<IBaseResponse<CarViewModel>> GetCar(int id);

        Task<BaseResponse<Dictionary<int, string>>> GetCar(string term);

        Task<IBaseResponse<Car>> Create(CarViewModel carViewModel, byte[] imageData);

        Task<IBaseResponse<bool>> DeleteCar(int id);

        Task<IBaseResponse<Car>> Edit(int id, CarViewModel model);
        BaseResponse<Dictionary<int, string>> GetTypes();
    }
}
