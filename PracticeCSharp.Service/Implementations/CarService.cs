using Microsoft.EntityFrameworkCore;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Enum;
using PracticeCSharp.Domain.Extensions;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Car;
using PracticeCSharp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace PracticeCSharp.Service.Implementations
{
    public class CarService : ICarService
    {
        private readonly IBaseRepository<Car> _carRepository;

        public CarService(IBaseRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IBaseResponse<CarViewModel>> GetCar(long id)
        {
            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (car == null)
                {
                    return new BaseResponse<CarViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new CarViewModel()
                {
                    Id = car.Id,
                    DateCreate = car.DateCreate.ToString("yyyy-MM-dd"),
                    Description = car.Description,
                    Name = car.Name,
                    Price = car.Price,
                    TypeCar = car.TypeCar.GetDisplayName(),
                    Speed = car.Speed,
                    Model = car.Model,
                    Image = car.Avatar,
                };

                return new BaseResponse<CarViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CarViewModel>()
                {
                    Description = $"[GetCar] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Dictionary<long, string>>> GetCar(string term)
        {
            var baseResponse = new BaseResponse<Dictionary<long, string>>();
            try
            {
                var cars = await _carRepository.GetAll()
                    .Select(x => new CarViewModel()
                    {
                        Id = x.Id,
                        Speed = x.Speed,
                        Name = x.Name,
                        Description = x.Description,
                        Model = x.Model,
                        DateCreate = x.DateCreate.ToString("yyyy-MM-dd"),
                        Price = x.Price,
                        TypeCar = x.TypeCar.GetDisplayName(),
                        Image = x.Avatar
                    })
                    .Where(x => EF.Functions.Like(x.Name, $"%{term}%"))
                    .ToDictionaryAsync(x => x.Id, t => t.Name);

                baseResponse.Data = cars;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<long, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Car>> Create(CarViewModel carViewModel)
        {
            try
            {
                var car = new Car()
                {
                    Description = carViewModel.Description,
                    DateCreate = DateTime.Parse(carViewModel.DateCreate),
                    Speed = carViewModel.Speed,
                    Model = carViewModel.Model,
                    Price = carViewModel.Price,
                    Name = carViewModel.Name,
                    TypeCar = (TypeCar)Convert.ToInt32(carViewModel.TypeCar),
                    Avatar = carViewModel.Image
                };

                await _carRepository.Create(car);

                return new BaseResponse<Car>()
                {
                    StatusCode = StatusCode.OK,
                    Data = car
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[DeleteCar] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteCar(long id)
        {
            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (car == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Машина не найдена",
                        StatusCode = StatusCode.CarNotFound,
                        Data = false
                    };
                }
                
                await _carRepository.Delete(car);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteCar] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Car>> GetCars()
        {
            try
            {
                var cars = _carRepository.GetAll().ToList();
                if (!cars.Any())
                {
                    return new BaseResponse<List<Car>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }


                return new BaseResponse<List<Car>>()
                {
                    Data = cars,
                    StatusCode = StatusCode.OK
                };

            }
            catch(Exception ex)
            {
                return new BaseResponse<List<Car>>()
                {
                    Description = $"[GetCars] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Car>> Edit(long id, CarViewModel model)
        {
            var baseResponse = new BaseResponse<Car>();
            try
            {
                var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (car == null) 
                {
                    baseResponse.StatusCode = StatusCode.CarNotFound;
                    baseResponse.Description = "Car not found";
                    return baseResponse;
                }

                car.Name = model.Name;
                car.Description = model.Description;
                car.Model = model.Model;
                car.Speed = model.Speed;
                car.Price = model.Price;
                car.DateCreate = DateTime.Parse(model.DateCreate);
                car.TypeCar = (TypeCar)Convert.ToInt32(model.TypeCar);
                car.Avatar = model.Image;

                await _carRepository.Update(car);


                return baseResponse;
                
            }
            catch(Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[GetCars] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Dictionary<long, string>> GetTypes()
        {
            try
            {
                var types = ((TypeCar[])Enum.GetValues(typeof(TypeCar)))
                    .ToDictionary(k => (long)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<long, string>>()
                {
                    Data = types,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<long, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
