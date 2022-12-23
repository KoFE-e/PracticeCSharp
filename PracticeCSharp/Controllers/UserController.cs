using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Extensions;
using PracticeCSharp.Domain.ViewModels.User;
using PracticeCSharp.Service.Implementations;
using PracticeCSharp.Service.Interfaces;

namespace PracticeCSharp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetUsers();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteUser(long id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetUsers");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Save() => PartialView();
        [HttpGet]
        public async Task<IActionResult> GetUser(long id, bool isJson)
        {
            var response = await _userService.GetUser(id);
            if (isJson)
            {
                return Json(response.Data);
            }
            return PartialView("Save", response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserViewModel model)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var response = await _userService.Create(model);
                    if (response.StatusCode == Domain.Enum.StatusCode.OK)
                    {
                        return Json(new { description = response.Description });
                    }
                    return BadRequest(new { errorMessage = response.Description });
                }
                else
                {
                    var response = await _userService.Edit(model.Id, model);
                    if (response.StatusCode == Domain.Enum.StatusCode.OK)
                    {
                        return Json(new { description = response.Description });
                    }
                    return BadRequest(new { errorMessage = response.Description });
                }   
            }
            var errorMessage = ModelState.Values
                .SelectMany(v => v.Errors.Select(x => x.ErrorMessage)).ToList().Aggregate((a,x) => a + '\n' + x);
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage });
        }

        [HttpPost]
        public JsonResult GetRoles()
        {
            var types = _userService.GetRoles();
            return Json(types.Data);
        }
    }
}