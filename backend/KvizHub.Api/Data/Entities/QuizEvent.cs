using KvizHub.Shared.Enums;

namespace KvizHub.Api.Data.Entities
{
    public class QuizEvent
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public AppUser Organizer { get; set; } = null!;
        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;
        public required string Title { get; set; }
        public DateTime StartsAt { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Draft;
        public int Capacity { get; set; }
        public bool JokerEnabled { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public DateTime? FeedbackClosesAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
