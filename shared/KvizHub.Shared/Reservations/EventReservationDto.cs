using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Reservations;

public record EventReservationDto(
    int Id, 
    string TeamName, 
    int TeamSize, 
    string LeaderName, 
    ReservationStatus Status
);