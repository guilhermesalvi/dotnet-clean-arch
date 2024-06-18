namespace CleanArch.Application.Notifications;

public readonly record struct Notification(string Key, string Value)
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
