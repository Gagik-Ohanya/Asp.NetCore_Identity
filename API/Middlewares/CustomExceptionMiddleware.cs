using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BLL.Exceptions;
using API.Models;
using System.Net;
using Serilog;

namespace API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomException ex)
            {
                Log.Error(ex, ex.Message);
                if (httpContext.Response.HasStarted)
                    throw;

                await HandleException(httpContext, ex.StatusCode, ex.Message);
                return;
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
                if (httpContext.Response.HasStarted)
                    throw;

                await HandleException(httpContext, (int)HttpStatusCode.InternalServerError, "Internal server error");
                return;
            }
        }

        private async Task HandleException(HttpContext httpContext, int statusCode, string message)
        {
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message
            });
            await httpContext.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}