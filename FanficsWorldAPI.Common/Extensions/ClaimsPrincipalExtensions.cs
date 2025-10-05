using System.Security.Claims;

namespace FanficsWorldAPI.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user) =>
            user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }
}
