using System.Security.Claims;

namespace Api.Common.Extensions
{
    public static class ClaimsExtension
    {
        public static string? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
