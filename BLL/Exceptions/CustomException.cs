using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"application/json";

        public CustomException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public CustomException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}