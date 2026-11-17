using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Auth;

public record AuthUserDto(string Email, string DisplayName, UserRole Role);