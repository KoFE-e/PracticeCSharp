using System.Security.Claims;
using System.Threading.Tasks;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Account;

namespace PracticeCSharp.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
    }
}