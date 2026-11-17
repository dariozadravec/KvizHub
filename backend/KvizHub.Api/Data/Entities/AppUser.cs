using KvizHub.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace KvizHub.Api.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public required string DisplayName { get; set; }
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
