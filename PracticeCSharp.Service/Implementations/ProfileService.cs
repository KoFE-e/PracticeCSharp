using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Enum;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Profile;
using PracticeCSharp.Service.Interfaces;

namespace Automarket.Service.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IBaseRepository<Profile> _profileRepository;

        public ProfileService(IBaseRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<BaseResponse<ProfileViewModel>> GetProfile(string userName)
        {
            try
            {
                var profile = await _profileRepository.GetAll()
                    .Select(x => new ProfileViewModel()
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Age = x.Age,
                        UserName = x.User.Name
                    })
                    .FirstOrDefaultAsync(x => x.UserName == userName);

                return new BaseResponse<ProfileViewModel>()
                {
                    Data = profile,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProfileViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<Profile>> Save(ProfileViewModel model)
        {
            try
            {
                var profile = await _profileRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                profile.Address = model.Address;
                profile.Age = model.Age;

                await _profileRepository.Update(profile);

                return new BaseResponse<Profile>()
                {
                    Data = profile,
                    Description = "Данные обновлены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Profile>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
    }
}