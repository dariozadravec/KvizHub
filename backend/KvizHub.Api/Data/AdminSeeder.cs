using KvizHub.Api.Data.Entities;
using KvizHub.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace KvizHub.Api.Data
{
    public static class AdminSeeder
    {
        // Creates the administrator account from configuration if it does not exist yet
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
        {
            var email = configuration["Admin:Email"];
            var password = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.LogWarning("Administrator account is not configured (Admin:Email, Admin:Password), skipping.");
                return;
            }

            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (await userManager.FindByEmailAsync(email) is not null)
                return;

            var admin = new AppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                DisplayName = configuration["Admin:DisplayName"] ?? "Administrator",
                Role = UserRole.Admin
            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create the administrator account: " +
                    string.Join(" ", result.Errors.Select(e => e.Description)));
        }
    }
}