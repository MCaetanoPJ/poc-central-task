using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace CentralTask.Core.Extensions;

public static class HttpContextExtensions
{
    public static T? ObterService<T>(this HttpContext httpContext)
    {
        var service = httpContext.RequestServices.GetService(typeof(T));
        return service is null ? default : (T)service;
    }

    public static bool EstaAutenticado(this HttpContext httpContext)
    {
        return httpContext.User.Identity is { IsAuthenticated: true };
    }

    public static string? ObterUserId(this HttpContext httpContext)
    {
        var claim = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }

    public static string? ObterBearerToken(this HttpContext httpContext)
    {
        if (!httpContext.EstaAutenticado())
        {
            return null;
        }
        ;

        var token = httpContext.Request.Headers[HeaderNames.Authorization].ToString();
        token = token.Replace("Bearer", string.Empty).Trim();
        return token;
    }

    public static IEnumerable<Claim> ObterClaimsToken(this HttpContext httpContext)
    {
        if (!httpContext.EstaAutenticado())
        {
            return new List<Claim>();
        }

        return httpContext.User.Claims;
    }

    public static string? ObterPerfil(this HttpContext httpContext)
    {
        if (!httpContext.EstaAutenticado())
        {
            return null;
        }

        var claim = httpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        return claim?.Value;
    }
}
