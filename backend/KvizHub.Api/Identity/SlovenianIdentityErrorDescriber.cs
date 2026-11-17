using Microsoft.AspNetCore.Identity;

namespace KvizHub.Api.Identity
{
    // Slovenian messages for the Identity errors that users can see
    public class SlovenianIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email) =>
            new() { Code = nameof(DuplicateEmail), Description = "Račun s tem e-naslovom že obstaja." };

        // The user name is the email, so a duplicate user name means the same thing; user name is not used in the actual app
        public override IdentityError DuplicateUserName(string userName) =>
            new() { Code = nameof(DuplicateUserName), Description = "Račun s tem e-naslovom že obstaja." };

        public override IdentityError InvalidEmail(string? email) =>
            new() { Code = nameof(InvalidEmail), Description = "E-naslov ni veljaven." };

        public override IdentityError PasswordTooShort(int length) =>
            new() { Code = nameof(PasswordTooShort), Description = $"Geslo mora imeti vsaj {length} znakov." };

        public override IdentityError PasswordRequiresDigit() =>
            new() { Code = nameof(PasswordRequiresDigit), Description = "Geslo mora vsebovati vsaj eno številko." };
    }
}