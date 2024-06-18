using System.Collections.Immutable;
using Microsoft.Extensions.Localization;

namespace CleanArch.Application.Notifications;

public sealed class NotificationManager(IStringLocalizer localizer)
{
    private readonly HashSet<Notification> _notifications = [];

    public IImmutableSet<Notification> Notifications => _notifications.ToImmutableHashSet();
    public bool HasNotifications => _notifications.Count != 0;

    public void AddNotification(Notification notification) =>
        _notifications.Add(notification);

    public void AddNotification(string key, params object[] valueParams)
    {
        var localized = localizer[key, valueParams];
        var notification = new Notification(key, localized);
        AddNotification(notification);
    }
}
