using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Conversations.API.Common.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception e)
            {
                var result = string.Empty;
                var code = HttpStatusCode.InternalServerError;
                if (e is ValidationException validationException)
                {
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Errors);
                }
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) code;
                if (result ==  string.Empty)
                {
                    result = JsonConvert.SerializeObject(new {Error = e.Message});
                }
                await context.Response.WriteAsync(result);
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}