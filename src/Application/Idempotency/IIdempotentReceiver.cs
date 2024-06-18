namespace CleanArch.Application.Idempotency;

public interface IIdempotentReceiver
{
    Task<bool> IsProcessedAsync(Guid key, CancellationToken cancellationToken);
    Task SetProcessedAsync(Guid key, CancellationToken cancellationToken);
    Task UpdateResourceAsync(Guid key, string resource, CancellationToken cancellationToken);
    Task<string?> GetResourceAsync(Guid key, CancellationToken cancellationToken);
    Task SetUnprocessedAsync(Guid key, CancellationToken cancellationToken);
}
