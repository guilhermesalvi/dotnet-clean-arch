using CleanArch.Application.Idempotency;
using CleanArch.Domain.AuditEventProcessor;
using CleanArch.Infrastructure.Data.Contexts;
using CleanArch.Infrastructure.Data.Repositories;
using CleanArch.Infrastructure.Data.Security;
using CleanArch.Infrastructure.Data.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Infrastructure.Data.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddContexts(configuration)
            .AddCaching(configuration)
            .AddSecurityServices()
            .AddRepositories();
    }

    private static IServiceCollection AddContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<DataSettings>()
            .BindConfiguration(nameof(DataSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.Get<DataSettings>() ??
                       throw new InvalidOperationException("DataSettings is required");

        services.AddDbContext<CleanArchDbContext>(options =>
            options.UseSqlServer(settings.ConnectionString));

        return services;
    }

    private static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<CacheSettings>()
            .BindConfiguration(nameof(CacheSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = configuration.Get<CacheSettings>() ??
                       throw new InvalidOperationException("CacheSettings is required");

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = settings.ConnectionString;
            options.SchemaName = settings.SchemaName;
            options.TableName = settings.TableName;
        });

        return services;
    }

    private static IServiceCollection AddSecurityServices(
        this IServiceCollection services)
    {
        return services
            .AddScoped<DataSecurityService>();
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        return services
            .AddScoped<IAuditEventRepository, AuditEventRepository>()
            .AddScoped<IIdempotentReceiver, IdempotentReceiver>();
    }
}
