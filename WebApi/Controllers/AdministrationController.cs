using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Exceptions;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ApiResponseBase> CreateRole(ApiCreateRoleRequest input)
        {
            var identityRole = new IdentityRole
            {
                Name = input.RoleName
            };
            IdentityResult result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, result.Errors.First().Description);
            
            return new ApiResponseBase
            {
                Succeeded = true
            };
        }

        [HttpPut]
        public async Task<ApiEditRoleResponse> EditRole(ApiRole inputRole)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(inputRole.Id);
            if (role == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "Role doesn't exist!");
            role.Name = inputRole.Name;
            IdentityResult result = await _roleManager.UpdateAsync(role);
            if(!result.Succeeded)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, result.Errors.First().Description);

            return new ApiEditRoleResponse
            {
                Succeeded = true,
                Role = new ApiRole
                {
                    Id = role.Id,
                    Name = role.Name
                }
            };
        }

        [HttpGet]
        public IEnumerable<ApiRole> GetRoles()
        {
            return _roleManager.Roles.Select(r => new ApiRole
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        [HttpPost]
        public async Task<ApiResponseBase> AddUserToRole(ApiUserToRoleRequest input)
        {
            IdentityUser user = await _userManager.FindByIdAsync(input.UserId);
            if (user == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "User not found!");
            
            IdentityRole role = await _roleManager.FindByIdAsync(input.RoleId);
            if (role == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "Role not found!");

            IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, result.Errors.First().Description);

            return new ApiResponseBase { Succeeded = true };
        }

        [HttpPost]
        public async Task<ApiResponseBase> RemoveUserFromRole(ApiUserToRoleRequest input)
        {
            IdentityUser user = await _userManager.FindByIdAsync(input.UserId);
            IdentityRole role = await _roleManager.FindByIdAsync(input.RoleId);

            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, result.Errors.First().Description);

            return new ApiResponseBase { Succeeded = true };
        }
    }
}