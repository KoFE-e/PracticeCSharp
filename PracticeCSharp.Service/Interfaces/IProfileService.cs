using System.Collections.Generic;
using System.Threading.Tasks;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Profile;

namespace PracticeCSharp.Service.Interfaces
{
    public interface IProfileService
    {
        Task<BaseResponse<ProfileViewModel>> GetProfile(string userName);

        Task<BaseResponse<Profile>> Save(ProfileViewModel model);
    }
}