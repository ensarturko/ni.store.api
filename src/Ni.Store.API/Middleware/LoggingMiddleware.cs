using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Ni.Store.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next, IHostingEnvironment hostingEnvironment)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();

            context.Response.OnStarting(() => {
                stopwatch.Stop();

                context.Response.Headers.Add("Elapsed", new[] { stopwatch.ElapsedMilliseconds.ToString() });
                context.Response.Headers.Add("Environment", new[] { _hostingEnvironment.EnvironmentName });

                var deployment = Environment.GetEnvironmentVariable("DEPLOYMENT_INFO");

                if (!string.IsNullOrEmpty(deployment))
                {
                    context.Response.Headers.Add("Deployment", new[] { deployment });
                }

                if (context.Request.Headers["TransactionId"].Any())
                {
                    context.Response.Headers.Add("TransactionId", context.Request.Headers["TransactionId"]);
                }

                return Task.CompletedTask;
            });

            stopwatch.Start();

            await _next(context);
        }
    }
}
