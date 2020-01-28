using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi
{
    public static class Mapper
    {
        public static BllIdentity ToBllIdentity(this ApiIdentity item)
        {
            return new BllIdentity
            {
                Username = item.Username,
                Password = item.Password,
                Email = item.Email,
                FirstName = item.Username,
                LastName = item.Username,
                Gender = item.Gender
            };
        }

        public static BllLoginUser ToBllLoginUser(this ApiIdentity item)
        {
            return new BllLoginUser
            {
                Username = item.Username,
                Password = item.Password,
            };
        }

        public static BllLoginUser ToBllLoginUser(this ApiLoginRequest item)
        {
            return new BllLoginUser
            {
                Username = item.Username,
                Password = item.Password,
            };
        }
    }
}