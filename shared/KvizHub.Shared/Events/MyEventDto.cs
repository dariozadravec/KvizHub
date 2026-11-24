using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Events;

public record MyEventDto(
    int Id, string Title, DateTime StartsAt, EventStatus Status, string VenueName, string VenueCity
);