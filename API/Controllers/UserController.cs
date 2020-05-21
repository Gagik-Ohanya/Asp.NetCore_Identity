using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ApiLoginResponse Register(ApiIdentity input)
        {
            int userId = _userService.Create(input.ToBllIdentity());
            string token = _userService.LogIn(input.ToBllLoginUser());
            return new ApiLoginResponse
            {
                UserId = userId,
                Succeeded = true,
                Username = input.Username,
                Token = token
            };
        }

        [HttpPost]
        public ApiLoginResponse Login(ApiLoginRequest input)
        {
            string token = _userService.LogIn(input.ToBllLoginUser());
            return new ApiLoginResponse
            {
                Succeeded = true,
                Username = input.Username,
                Token = token
            };
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ApiCreateRoleResponse CreateRole(ApiCreateRoleRequest input)
        {
            int roleId = _userService.CreateRole(input.RoleName);
            return new ApiCreateRoleResponse
            {
                Succeeded = true,
                RoleId = roleId,
                RoleName = input.RoleName
            };
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ApiResponseBase AddUserToRole(ApiUserToRoleRequest input)
        {
            int userId = Convert.ToInt32(input.UserId);
            int roleId = Convert.ToInt32(input.RoleId);
            _userService.AddUserToRole(userId, roleId);
            
            return new ApiResponseBase
            {
                Succeeded = true
            };
        }

        [HttpPost]
        public async Task<IActionResult> TestCache(string value)
        {
            await _userService.TestCache(value);
            return Ok();
        }
    }
}