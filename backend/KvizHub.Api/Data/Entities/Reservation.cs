using KvizHub.Shared.Enums;

namespace KvizHub.Api.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public QuizEvent Event { get; set; } = null!;
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public int LeaderId { get; set; }
        public TeamMember Leader { get; set; } = null!;
        public short TeamSize { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
        public int TiebreakPoints { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CancelledAt { get; set; }
        public Feedback? Feedback { get; set; }
    }
}
