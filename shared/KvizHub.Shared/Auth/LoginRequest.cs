using System.ComponentModel.DataAnnotations;

namespace KvizHub.Shared.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "E-naslov je obvezen.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Geslo je obvezno.")]
    public string Password { get; set; } = "";
}