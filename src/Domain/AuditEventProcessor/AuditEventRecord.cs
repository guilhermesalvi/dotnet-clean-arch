namespace CleanArch.Domain.AuditEventProcessor;

public record AuditEventRecord(
    Guid Id,
    Guid AggregateId,
    Guid CorrelationId,
    Guid UserId,
    string EventName,
    string Data)
{
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
