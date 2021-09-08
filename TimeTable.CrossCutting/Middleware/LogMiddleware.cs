using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TimeTable.CrossCutting.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = UriHelper.GetDisplayUrl(context.Request);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

            await _next(context);

            context.Request.Body = originalRequestBody;
            TraceRequest(requestBodyText, url, context.Request.Method);
        }

        private void TraceRequest(string payload, string url, string method)
        {
            System.Diagnostics.Debug.WriteLine($"***** LOG:{Environment.NewLine}          Url => {url};{Environment.NewLine}          Body => {payload};{Environment.NewLine}          Method => {method}");
        }
    }
}
