using KvizHub.Shared.Enums;

namespace KvizHub.Client.Helpers
{
    // Slovenian texts for reservation states shown in the user interface
    public static class ReservationStatusText
    {
        public static string Label(ReservationStatus status) => status switch
        {
            ReservationStatus.Confirmed => "Potrjena",
            ReservationStatus.Attended => "Prisotna",
            ReservationStatus.Waitlisted => "Na čakalni listi",
            ReservationStatus.NoShow => "Odsotna",
            ReservationStatus.Cancelled => "Preklicana",
            _ => status.ToString()
        };
    }
}