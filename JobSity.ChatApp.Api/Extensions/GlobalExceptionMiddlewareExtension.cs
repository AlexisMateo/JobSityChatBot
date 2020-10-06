using JobSity.ChatApp.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace JobSity.ChatApp.Api.Extensions
{
    public static class GlobalExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}