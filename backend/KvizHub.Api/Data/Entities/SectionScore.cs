namespace KvizHub.Api.Data.Entities
{
    public class SectionScore
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        public int SectionId { get; set; }
        public Section Section { get; set; } = null!;
        public decimal Points { get; set; }
        public bool IsJoker { get; set; }
    }
}
