namespace CleanArch.Domain.SeedWork.Models;

public abstract class Entity<TKey> where TKey : struct, IEquatable<TKey>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TKey Id { get; protected set; }

    protected Entity(TKey id)
    {
        if (id.Equals(default)) throw new ArgumentNullException(nameof(id));
        Id = id;
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public bool SameIdentityAs(Entity<TKey>? other) => other is not null && Id.Equals(other.Id);

    public void Append(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    public void Remove(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void Clear() => _domainEvents.Clear();
}
