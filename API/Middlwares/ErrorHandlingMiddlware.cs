using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Middlwares
{
    public class ErrorHandlingMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddlware> _logger;

        public ErrorHandlingMiddlware(RequestDelegate next, ILogger<ErrorHandlingMiddlware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            object errors = null;

            switch (ex)
            {
                case RestException re:
                    _logger.LogError(ex, "REST ERROR");
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;

                case Exception e:
                    _logger.LogError(ex, "INTERNAL SERVER ERROR");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    break;
            }


            if (errors != null)
            {
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { errors });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
