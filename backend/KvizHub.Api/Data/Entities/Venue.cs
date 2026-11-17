namespace KvizHub.Api.Data.Entities
{
    public class Venue
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public required string NormalizedAddress { get; set; }
        public int DefaultCapacity { get; set; }
        public int CreatedById { get; set; }
        public AppUser CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
