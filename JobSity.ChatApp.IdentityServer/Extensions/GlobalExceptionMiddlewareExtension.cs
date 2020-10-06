using JobSity.ChatApp.IdentityServer.Middleware;
using Microsoft.AspNetCore.Builder;

namespace JobSity.ChatApp.IdentityServer.Extensions
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