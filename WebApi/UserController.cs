using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi
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
        [Authorize]
        public IActionResult CreateRole()
        {
            return Ok();
        }
    }
}