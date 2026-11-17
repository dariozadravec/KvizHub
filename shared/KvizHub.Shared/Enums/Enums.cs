using System.Text.Json.Serialization;

namespace KvizHub.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<UserRole>))]
public enum UserRole { Quizmaster, TeamLeader, Admin }

[JsonConverter(typeof(JsonStringEnumConverter<EventStatus>))]
public enum EventStatus { Draft, Published, RegistrationsClosed, InProgress, Finished, FeedbackOpen, Archived }

[JsonConverter(typeof(JsonStringEnumConverter<ReservationStatus>))]
public enum ReservationStatus { Confirmed, Attended, Waitlisted, NoShow, Cancelled }