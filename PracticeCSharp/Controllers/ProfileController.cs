using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PracticeCSharp.Domain.ViewModels.Profile;
using PracticeCSharp.Service.Interfaces;

namespace PracticeCSharp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            ModelState.Remove("Id");
            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                var response = await _profileService.Save(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public async Task<IActionResult> Detail()
        {
            var userName = User.Identity.Name;
            var response = await _profileService.GetProfile(userName);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View();
        }
    }
}