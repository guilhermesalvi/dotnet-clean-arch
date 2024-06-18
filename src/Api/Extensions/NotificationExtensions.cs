using System.Globalization;
using System.Reflection;
using CleanArch.Api.Resources;
using CleanArch.Application.Notifications;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace CleanArch.Api.Extensions;

public static class NotificationExtensions
{
    private static readonly string[] SupportedCultures = ["pt-BR"];

    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        return services
            .Configure<RequestLocalizationOptions>(x =>
            {
                x.DefaultRequestCulture = new RequestCulture("pt-BR");
                x.SupportedCultures = SupportedCultures.Select(c => new CultureInfo(c)).ToList();
                x.SupportedUICultures = SupportedCultures.Select(c => new CultureInfo(c)).ToList();
            })
            .AddLocalization(x => x.ResourcesPath = "Resources")
            .AddScoped<NotificationManager>(sp =>
            {
                var type = typeof(SharedResource);
                var assemblyName = new AssemblyName(type.Assembly.FullName ?? string.Empty);
                var factory = sp.GetRequiredService<IStringLocalizerFactory>();
                var localizer = factory.Create(type.Name, assemblyName.Name ?? string.Empty);
                return new NotificationManager(localizer);
            });
    }

    public static IApplicationBuilder UseNotifications(this IApplicationBuilder app)
    {
        return app.UseRequestLocalization();
    }
}
