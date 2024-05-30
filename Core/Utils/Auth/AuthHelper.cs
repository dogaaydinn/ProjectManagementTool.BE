using Core.Extensions;
using Core.Utils.IoC;
using Microsoft.AspNetCore.Http;

namespace Core.Utils.Auth;

public static class AuthHelper
{
    private static readonly IHttpContextAccessor HttpContextAccessor = ServiceTool.GetService<IHttpContextAccessor>()!;

    public static Guid? GetUserId()
    {
          return HttpContextAccessor.HttpContext!.GetUserId();
    }

    public static string? GetEmail()
    {
        return HttpContextAccessor.HttpContext!.GetEmail();
    }

    public static string? GetUsername()
    {
        return HttpContextAccessor.HttpContext!.GetUsername();
    }


    public static string? GetRole()
    {
        return HttpContextAccessor.HttpContext!.GetRole();
    }

    public static bool IsLoggedIn()
    {
        return HttpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
    }
}