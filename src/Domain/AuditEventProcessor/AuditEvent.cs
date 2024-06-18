using MediatR;

namespace CleanArch.Domain.AuditEventProcessor;

public record AuditEvent<T>(
    Guid AggregateId,
    Guid CorrelationId,
    Guid UserId,
    string EventName,
    T Data) : INotification;
