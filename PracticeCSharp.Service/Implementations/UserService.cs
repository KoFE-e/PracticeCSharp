using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Automarket.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.DAL.Repositories;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Enum;
using PracticeCSharp.Domain.Extensions;
using PracticeCSharp.Domain.Helpers;
using PracticeCSharp.Domain.Response;
using PracticeCSharp.Domain.ViewModels.Car;
using PracticeCSharp.Domain.ViewModels.User;
using PracticeCSharp.Service.Interfaces;


namespace PracticeCSharp.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Profile> _proFileRepository;

        public UserService(IBaseRepository<User> userRepository, IBaseRepository<Profile> proFileRepository)
        {
            _userRepository = userRepository;
            _proFileRepository = proFileRepository;
        }

        public async Task<IBaseResponse<User>> Create(UserViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
                if (user != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь с таким логином уже есть",
                        StatusCode = StatusCode.UserAlreadyExists
                    };
                }
                user = new User()
                {
                    Name = model.Name,
                    Role = Enum.Parse<Role>(model.Role),
                    Password = HashPasswordHelper.HashPassword(model.Password),
                };

                await _userRepository.Create(user);

                var profile = new Profile()
                {
                    Address = string.Empty,
                    Age = 0,
                    UserId = user.Id,
                };

                await _proFileRepository.Create(profile);

                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = "Пользователь добавлен",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<User>> Edit(long id, UserViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                user.Name = model.Name;
                user.Password = HashPasswordHelper.HashPassword(model.Password);
                user.Role = (Role)Convert.ToInt32(model.Role);


                await _userRepository.Update(user);


                return new BaseResponse<User>()
                {
                    Description = "Данные изменены",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $"[EditUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<UserViewModel>> GetUser(long id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<UserViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var data = new UserViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = user.Password,
                    Role = user.Role.GetDisplayName()
                };

                return new BaseResponse<UserViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserViewModel>()
                {
                    Description = $"[GetUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetRoles()
        {
            try
            {
                var roles = ((Role[])Enum.GetValues(typeof(Role)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = roles,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAll()
                    .Select(x => new UserViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Role = x.Role.GetDisplayName()
                    })
                    .ToListAsync();

                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    Data = users,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteUser(long id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Data = false
                    };
                }
                await _userRepository.Delete(user);

                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        private ClaimsIdentity Authentificate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}