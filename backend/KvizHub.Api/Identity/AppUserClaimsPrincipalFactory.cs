using System.Security.Claims;
using KvizHub.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace KvizHub.Api.Identity
{
    // Adds the role from app_user.role to the session cookie, so role-based authorization works
    public class AppUserClaimsPrincipalFactory(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<IdentityOptions> options)
        : UserClaimsPrincipalFactory<AppUser, IdentityRole<int>>(userManager, roleManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            return identity;
        }
    }
}
