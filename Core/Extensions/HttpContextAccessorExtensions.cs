using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions;

public static class HttpContextAccessorExtensions
{
    public static string? GetUserId(this HttpContext httpContext)
    {
        return httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    public static string? GetEmail(this HttpContext httpContext)
    {
        return httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    }

    public static string? GetUsername(this HttpContext httpContext)
    {
        return httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }

    public static string? GetRole(this HttpContext httpContext)
    {
        return httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }
}