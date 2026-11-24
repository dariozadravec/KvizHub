using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Events;

public static class EventStatusFlow
{
    // States the quizmaster moves an event through, one step at a time (US-04 K1).
    // Later states (feedback, archive) are set by the system.
    private static readonly EventStatus[] ManualFlow =
    [
        EventStatus.Draft,
        EventStatus.Published,
        EventStatus.RegistrationsClosed,
        EventStatus.InProgress,
        EventStatus.Finished
    ];

    // Next state the quizmaster can move the event to, or null if there is none
    public static EventStatus? Next(EventStatus current)
    {
        var index = Array.IndexOf(ManualFlow, current);
        return index >= 0 && index < ManualFlow.Length - 1 ? ManualFlow[index + 1] : null;
    }
}
