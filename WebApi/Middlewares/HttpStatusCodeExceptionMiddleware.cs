using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApi.Exceptions;

namespace WebApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpStatusCodeException ex)
            {
                if (httpContext.Response.HasStarted)
                    throw;

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex.StatusCode;
                httpContext.Response.ContentType = ex.ContentType;

                await httpContext.Response.WriteAsync(ex.Message);

                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
        }
    }
}