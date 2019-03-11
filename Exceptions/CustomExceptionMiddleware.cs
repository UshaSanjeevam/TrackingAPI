using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrackingAPI.Exceptions
{
    public class CustomExceptionMiddleware
    {
     
            private readonly RequestDelegate _next;

            public CustomExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next.Invoke(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private async Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
            //var response = context.Response;
            //var customException = exception as AppExceptions;
            //var statusCode = (int)HttpStatusCode.InternalServerError;
            //var message = "Unexpected error";
            //var description = "Unexpected error";

            //if (null != customException)
            //{
            //    message = customException.Message;
            //    description = customException.Description;
            //    statusCode = customException.Code;
            //}

            //response.ContentType = "application/json";
            //response.StatusCode = statusCode;
            //await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            //{
            //    Message = message,
            //    Description = description,
            //    StatusCode = statusCode
            //}));
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            //var ex = context.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                var err = JsonConvert.SerializeObject(new CustomErrorResponse()
                {
                  
                    Source =exception.Source,
                    LogTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt"),
                    // StackTrace = exception.StackTrace,
                    Message = exception.Message,
                    StatusCode=context.Response.StatusCode,
                    ControllerName= exception.TargetSite.ReflectedType.FullName,
                    Method= exception.TargetSite.Name
                });
                await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(err), 0, err.Length).ConfigureAwait(false);
                //var err = message + " " + exception.StackTrace;
                //response.WriteAsync(err);
            }
        }
     }

        
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}

