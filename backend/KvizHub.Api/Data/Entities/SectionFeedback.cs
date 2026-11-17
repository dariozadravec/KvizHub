namespace KvizHub.Api.Data.Entities
{
    public class SectionFeedback
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; } = null!;
        public int SectionId { get; set; }
        public Section Section { get; set; } = null!;
        public short Rating { get; set; }
        public string? Comment { get; set; }
    }
}
