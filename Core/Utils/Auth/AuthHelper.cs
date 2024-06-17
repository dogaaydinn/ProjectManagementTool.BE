using Core.Extensions;
using Core.Services.Messages;
using Core.Utils.IoC;
using Microsoft.AspNetCore.Http;

namespace Core.Utils.Auth;

public static class AuthHelper
{
    private static readonly IHttpContextAccessor HttpContextAccessor = ServiceTool.GetService<IHttpContextAccessor>()!;

    public static Guid? GetUserId()
    {
        return HttpContextAccessor.HttpContext!.User.GetUserId();
    }

    public static string? GetEmail()
    {
        return HttpContextAccessor.HttpContext!.User.GetEmail();
    }

    public static string? GetUsername()
    {
        return HttpContextAccessor.HttpContext!.User.GetUsername();
    }

    public static string? GetRole()
    {
        return HttpContextAccessor.HttpContext!.User.GetRole();
    }

    public static bool IsLoggedIn()
    {
        return HttpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
    }

    public static bool IsUserAuthorized(out ErrorMessage errorMessage, string requiredRole = null)
    {
        if (!IsLoggedIn())
        {
            errorMessage = new ErrorMessage("USER-00001", "User is not logged in");
            return false;
        }

        if (requiredRole != null && GetRole() != requiredRole)
        {
            errorMessage = new ErrorMessage("USER-00002", "User is not authorized");
            return false;
        }

        errorMessage = null;
        return true;
    }

    public static bool HasProjectAccess(Guid projectId)
    {
        var userProjects = HttpContextAccessor.HttpContext!.User.GetProjects();
        return userProjects.Contains(projectId);
    }
}