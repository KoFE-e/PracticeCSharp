using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Enum;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Car;
using PracticeCSharp.Service.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace PracticeCSharp.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public IActionResult GetCars()
        {
            
            var response = _carService.GetCars();
            if (response.StatusCode == Domain.Enum.StatusCode.OK) {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }

        public IActionResult Compare() => PartialView();

        [HttpGet]
        public async Task<ActionResult> GetCar(int id, bool isJson)
        {
            var response = await _carService.GetCar(id);
            if (isJson)
            {
                return Json(response.Data);
            }
            return PartialView("GetCar", response.Data);
        }

        public IActionResult WithValidation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCar(string term, int page = 1, int pageSize = 5)
        {
            var response = await _carService.GetCar(term);
            return Json(response.Data);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _carService.DeleteCar(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetCars");
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0) 
            {
                return PartialView();
            }
            var response = await _carService.GetCar(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }

            ModelState.AddModelError("", response.Description);
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromForm]CarViewModel car)
        {
            ModelState.Remove("Image");
            ModelState.Remove("Id");
            
            if (ModelState.IsValid)
            {
                if (car.Id == 0)
                {
                    if (car.Avatar != null)
                    {
                        byte[] imageData;
                        using (var binaryReader = new BinaryReader(car.Avatar.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)car.Avatar.Length);
                        }
                        await _carService.Create(car, imageData);
                    } 
                    else
                    {
                        await _carService.Create(car, null);
                    }
                }
                else
                {
                    await _carService.Edit(car.Id, car);
                }

                return View("GetCars");
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetTypes()
        {
            var types = _carService.GetTypes();
            return Json(types.Data);
        }
    }
}
