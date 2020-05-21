using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class HttpResponseExtension
    {
        public static void SetHttpResponse(this HttpResponse response, int statusCode)
        {
            response.OnStarting(() =>
            {
                response.StatusCode = statusCode;
                return Task.CompletedTask;
            });
        }
    }
}