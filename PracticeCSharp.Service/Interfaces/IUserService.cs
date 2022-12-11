﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PracticeCSharp.Domain.ViewModels.User;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Response;

namespace PracticeCSharp.Service.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<User>> Create(UserViewModel model);

        BaseResponse<Dictionary<int, string>> GetRoles();

        Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers();

        Task<IBaseResponse<bool>> DeleteUser(long id);
    }
}