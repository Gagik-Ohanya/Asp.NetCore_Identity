using BLL.Exceptions;
using BLL.Models;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtSettings _jwtSettings;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtSettings = appSettings.Value.JwtSettings;
        }

        public int Create(BllIdentity userIdentity)
        {
            var user = new User
            {
                Username = userIdentity.Username,
                Email = userIdentity.Email,
                EmailConfirmed = false,
                PasswordHash = CommonFunctions.ComputeMd5(userIdentity.Password + "salt"),
                FirstName = userIdentity.FirstName,
                LastName = userIdentity.LastName,
                Gender = userIdentity.Gender
            };
            _userRepository.AddUser(user);
            _userRepository.Commit();
            return user.Id;
        }

        public string LogIn(BllLoginUser userInput)
        {
            User user = Authenticate(userInput.Username, userInput.Password);
            return GenerateJWToken(user);
        }

        public User Authenticate(string username, string password)
        {
            string passwordHash = CommonFunctions.ComputeMd5(password + "salt");
            User user = _userRepository.Get(username, passwordHash);
            if (user == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "User not found!");
            return user;
        }

        private string GenerateJWToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var roles = _roleRepository.GetUserRoles(user.Id).Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };
            claims.AddRange(roles);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string strToken = tokenHandler.WriteToken(token);

            return strToken;
        }

        public int CreateRole(string name)
        {
            Role role = new Role
            {
                Name = name
            };
            _roleRepository.AddRole(role);
            _roleRepository.Commit();
            return role.Id;
        }

        public int AddUserToRole(int userId, int roleId)
        {
            User user = _userRepository.Get(userId);
            if (user == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "User not found!");
            Role role = _roleRepository.GetById(roleId);
            if (role == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "Role doesn't exist!");

            var userRole = new UserRole
            {
                User = user,
                Role = role
            };
            _roleRepository.AddUserRole(userRole);
            _roleRepository.Commit();
            return userRole.Id;
        }
    }
}