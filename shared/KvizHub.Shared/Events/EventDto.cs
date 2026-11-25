using KvizHub.Shared.Enums;
using KvizHub.Shared.Venues;

namespace KvizHub.Shared.Events;

public record EventDto(
    int Id,
    string Title,
    DateTime StartsAt,
    EventStatus Status,
    int Capacity,
    int FreeSeats,
    bool JokerEnabled,
    VenueDto Venue,
    string OrganizerName,
    List<SectionDto> Sections
);