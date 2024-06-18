using System.Text.Json;
using CleanArch.Domain.AuditEventProcessor;
using MediatR;

namespace CleanArch.Application.UseCases.AuditEventProcessor;

public class AuditEventHandler<T>(
    IAuditEventRepository repository) : INotificationHandler<AuditEvent<T>>
{
    public Task Handle(AuditEvent<T> notification, CancellationToken cancellationToken)
    {
        var record = new AuditEventRecord(
            Guid.NewGuid(),
            notification.AggregateId,
            notification.CorrelationId,
            notification.UserId,
            notification.EventName,
            JsonSerializer.Serialize(notification.Data));

        return repository.AppendAsync(record, cancellationToken);
    }
}
