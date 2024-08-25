using CleanArch.Domain.AuditEventProcessor;
using CleanArch.Infrastructure.Data.Contexts;

namespace CleanArch.Infrastructure.Data.Repositories;

public class AuditEventRepository(
    ApplicationDbContext context) : IAuditEventRepository
{
    public Task AppendAsync(AuditEventRecord auditEventRecord, CancellationToken cancellationToken)
    {
        context.AuditEventRecords.Add(auditEventRecord);
        return context.SaveChangesAsync(cancellationToken);
    }
}
