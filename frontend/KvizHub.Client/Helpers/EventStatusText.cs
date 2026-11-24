using KvizHub.Shared.Enums;

namespace KvizHub.Client.Helpers
{
    // Slovenian texts for event states shown in the user interface
    public static class EventStatusText
    {
        public static string Label(EventStatus status) => status switch
        {
            EventStatus.Draft => "Osnutek",
            EventStatus.Published => "Objavljen",
            EventStatus.RegistrationsClosed => "Rezervacije zaprte",
            EventStatus.InProgress => "V teku",
            EventStatus.Finished => "Zaključen",
            EventStatus.FeedbackOpen => "Ocenjevanje odprto",
            EventStatus.Archived => "Arhiviran",
            _ => status.ToString()
        };

        // Text of the button that moves the event to the given state
        public static string Action(EventStatus target) => target switch
        {
            EventStatus.Published => "Objavi",
            EventStatus.RegistrationsClosed => "Zapri rezervacije",
            EventStatus.InProgress => "Začni kviz",
            EventStatus.Finished => "Zaključi",
            _ => target.ToString()
        };
    }
}