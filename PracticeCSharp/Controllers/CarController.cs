using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public CarController(ICarService carService, IWebHostEnvironment hostEnvironment)
        {
            _carService = carService;
            this._hostEnvironment = hostEnvironment;
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
        public async Task<ActionResult> GetCar(long id, bool isJson)
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
        public async Task<IActionResult> Delete(long id)
        {
            //delete image from files
            var car = await _carService.GetCar(id);

            if(car.Data.Image != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img", car.Data.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            

            var response = await _carService.DeleteCar(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetCars");
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Save(long id)
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
        public async Task<IActionResult> Save(CarViewModel car)
        {
            ModelState.Remove("Image");
            ModelState.Remove("Id");

            if (car.Avatar != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(car.Avatar.FileName);
                string extension = Path.GetExtension(car.Avatar.FileName);
                car.Image = filename = filename + '_' + DateTime.Now.ToString("ddMMyyhhmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/img/", filename);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await car.Avatar.CopyToAsync(fileStream);
                }
            }
            
            if (ModelState.IsValid)
            {
                if (car.Id == 0)
                {
                    await _carService.Create(car);
                }
                else
                {
                    var model = await _carService.GetCar(car.Id);
                    if (model.Data.Image != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img", model.Data.Image);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
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
