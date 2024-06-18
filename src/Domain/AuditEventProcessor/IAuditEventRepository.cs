namespace CleanArch.Domain.AuditEventProcessor;

public interface IAuditEventRepository
{
    Task AppendAsync(AuditEventRecord auditEventRecord, CancellationToken cancellationToken);
}
