using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace TimeTable.Api.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public const string API_KEY_NAME = "x-api-key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_NAME, out StringValues extractedApiKey))
            {
                context.Result = GetInvalidApiKeyContentResult();
                return;
            }

            IConfiguration appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            string apiKey = appSettings.GetValue<string>("ApiKey");

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = GetInvalidApiKeyContentResult();
                return;
            }

            await next();
        }

        private ContentResult GetInvalidApiKeyContentResult()
        {
            return new ContentResult()
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            };
        }
    }
}
