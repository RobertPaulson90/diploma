using System;
using System.Net;
using System.Threading.Tasks;
using Diploma.WebAPI.Infrastructure.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Diploma.WebAPI.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex)
                    .ConfigureAwait(false);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is RestException re)
            {
                context.Response.StatusCode = (int)re.Code;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(
                    new
                    {
                        errors = exception.Message
                    });

                // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
                await context.Response.WriteAsync(result)
                    .ConfigureAwait(false);
            }
        }
    }
}
