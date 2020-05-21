using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL.Exceptions;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ApiLoginResponse> Register(ApiIdentity input)
        {
            var user = new IdentityUser
            {
                UserName = input.Username,
                Email = input.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);
            else
                throw new CustomException((int)HttpStatusCode.InternalServerError, result.Errors.First().Description);

            return new ApiLoginResponse
            {
                Succeeded = true,
                Username = input.Username
            };
        }

        [HttpPut]
        public async Task<ApiLoginResponse> Login(ApiLoginRequest input)
        {
            var result = await _signInManager.PasswordSignInAsync(input.Username, input.Password, isPersistent: false, false);
            if (!result.Succeeded)
                throw new CustomException((int)HttpStatusCode.InternalServerError, "Username or password is wrong!");

            return new ApiLoginResponse
            {
                Succeeded = true,
                Username = input.Username
            };
        }

        [HttpPut]
        public async Task<ApiResponseBase> Logout()
        {
            await _signInManager.SignOutAsync();
            return new ApiResponseBase
            {
                Succeeded = true
            };
        }
    }
}