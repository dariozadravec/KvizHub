namespace KvizHub.Shared.Events;

public record EventSearchResultDto(
    int Id,
    string Title,
    DateTime StartsAt,
    string VenueName,
    string VenueCity,
    string OrganizerName,
    int FreeSeats,
    bool JokerEnabled
);