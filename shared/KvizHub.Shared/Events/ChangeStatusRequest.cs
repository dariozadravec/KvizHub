using KvizHub.Shared.Enums;

namespace KvizHub.Shared.Events;

public record ChangeStatusRequest(EventStatus Status);