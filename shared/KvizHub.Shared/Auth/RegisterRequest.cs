using System.ComponentModel.DataAnnotations;
using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "E-naslov je obvezen.")]
    [EmailAddress(ErrorMessage = "E-naslov ni veljaven.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Prikazno ime je obvezno.")]
    public string DisplayName { get; set; } = "";

    [Required(ErrorMessage = "Geslo je obvezno.")]
    public string Password { get; set; } = "";

    public UserRole Role { get; set; } = UserRole.TeamLeader;
}