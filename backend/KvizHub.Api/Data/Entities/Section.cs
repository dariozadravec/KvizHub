namespace KvizHub.Api.Data.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public QuizEvent Event { get; set; } = null!;
        public short Position { get; set; }
        public required string Name { get; set; }
        public required string Topic { get; set; }
        public int MaxPoints { get; set; }
    }
}
