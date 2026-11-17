namespace KvizHub.Api.Data.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public required string Name { get; set; }
        public int? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
