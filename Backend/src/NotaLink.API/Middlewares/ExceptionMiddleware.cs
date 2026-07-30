using Microsoft.EntityFrameworkCore;
using NotaLink.Application.DTOs.API;
using System.ComponentModel.DataAnnotations;

namespace NotaLink.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var response = GetResponseByException(ex, context);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var response = GetResponseByException(ex, context);

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = GetResponseByException(ex, context);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
        private ErrorResponse GetResponseByException(Exception ex, HttpContext context)
        {
            var response = new ErrorResponse
            {
                Status = context.Response.StatusCode,
                Errors = new Dictionary<string, string[]>
                {
                    ["General"] = new[] { ex.Message }
                },
                TraceId = context.TraceIdentifier
            };

            return response;
        }
    }
    
}
