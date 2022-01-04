using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using TimeTable.Application.Exceptions;

namespace TimeTable.CrossCutting.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string errorDescription = null;
            string errorCode = null;

            switch (exception)
            {
                case NotValidOperationException notValidItemException:
                    statusCode = HttpStatusCode.Conflict;
                    errorDescription = notValidItemException.Description;
                    break;
                case ForbidenActionException forbidenActionException:
                    statusCode = HttpStatusCode.Forbidden;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            string errorBody = JsonConvert.SerializeObject(new { error_code = errorCode, error_description = errorDescription });
            return context.Response.WriteAsync(errorBody);
        }
    }
}
