using Microsoft.AspNetCore.Builder;

namespace TimeTable.CrossCutting.Middleware
{
    public static class MiddlewareManager
    {
        public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<LogMiddleware>();
            builder.UseMiddleware<ExceptionHandlerMiddleware>();
            return builder;
        }
    }
}
