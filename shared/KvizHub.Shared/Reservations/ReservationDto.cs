using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Reservations;

public record ReservationDto(
    int Id,
    int EventId,
    string EventTitle,
    DateTime EventStartsAt,
    string TeamName,
    int TeamSize,
    ReservationStatus Status
);