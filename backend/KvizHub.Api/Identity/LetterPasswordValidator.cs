using KvizHub.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace KvizHub.Api.Identity
{
    // Requires at least one letter
    public class LetterPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            if (password is not null && password.Any(char.IsLetter))
                return Task.FromResult(IdentityResult.Success);

            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordRequiresLetter",
                Description = "Geslo mora vsebovati vsaj eno črko."
            }));
        }
    }
}