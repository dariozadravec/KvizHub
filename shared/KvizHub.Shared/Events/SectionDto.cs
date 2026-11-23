namespace KvizHub.Shared.Events;

public record SectionDto(
    int Id, short Position, string Name, string Topic, int MaxPoints
);