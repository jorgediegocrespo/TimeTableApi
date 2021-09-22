using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
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
            var url = UriHelper.GetDisplayUrl(context.Request);            

            await _next(context);

            //TODO Trace body
            TraceRequest(url, context.Request.Method);
        }

        private void TraceRequest(string url, string method)
        {
            System.Diagnostics.Debug.WriteLine($"***** LOG:{Environment.NewLine}          Url => {url};{Environment.NewLine}          Method => {method}");
        }
    }
}
