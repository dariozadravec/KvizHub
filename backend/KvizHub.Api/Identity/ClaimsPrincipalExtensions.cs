using System.Security.Claims;

namespace KvizHub.Api.Identity
{
    public static class ClaimsPrincipalExtensions
    {
        // Returns id of the signed-in user, stored in the session cookie by Identity
        public static int GetUserId(this ClaimsPrincipal user) =>
            int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}