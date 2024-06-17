using System.Security.Claims;

namespace Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(userId, out var result) ? result : null;
    }

    public static string? GetEmail(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    }

    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }

    public static string? GetRole(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }

    public static IEnumerable<Guid> GetProjects(this ClaimsPrincipal user)
    {
        var projects = user.Claims.FirstOrDefault(c => c.Type == "Projects")?.Value;

        return projects?.Split(',').Select(Guid.Parse) ?? Enumerable.Empty<Guid>();
    }
}