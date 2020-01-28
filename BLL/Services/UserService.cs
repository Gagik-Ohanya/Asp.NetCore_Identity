using BLL.Exceptions;
using BLL.Models;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public UserService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
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
            return Authenticate(userInput);
        }

        public string Authenticate(BllLoginUser userInput)
        {
            string passwordHash = CommonFunctions.ComputeMd5(userInput.Password + "salt");
            User user = _userRepository.Get(userInput.Username, passwordHash);
            if (user == null)
                throw new HttpStatusCodeException((int)HttpStatusCode.InternalServerError, "User not found!");

            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            /*
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);
            */
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string strToken = tokenHandler.WriteToken(token);

            return strToken;
        }
    }
}