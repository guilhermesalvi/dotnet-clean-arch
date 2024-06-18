using CleanArch.Application.Idempotency;
using Microsoft.Extensions.Caching.Distributed;

namespace CleanArch.Infrastructure.Data.Repositories;

public class IdempotentReceiver(
    IDistributedCache cache) : IIdempotentReceiver
{
    private readonly DistributedCacheEntryOptions _options = new()
        { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3) };

    public async Task<bool> IsProcessedAsync(Guid key, CancellationToken cancellationToken) =>
        await cache.GetStringAsync(key.ToString(), cancellationToken) is not null;

    public Task SetProcessedAsync(Guid key, CancellationToken cancellationToken) =>
        cache.SetStringAsync(key.ToString(), string.Empty, _options, cancellationToken);

    public Task UpdateResourceAsync(Guid key, string resource, CancellationToken cancellationToken) =>
        cache.SetStringAsync(key.ToString(), resource, _options, cancellationToken);

    public Task<string?> GetResourceAsync(Guid key, CancellationToken cancellationToken) =>
        cache.GetStringAsync(key.ToString(), cancellationToken);

    public Task SetUnprocessedAsync(Guid key, CancellationToken cancellationToken) =>
        cache.RemoveAsync(key.ToString(), cancellationToken);
}
