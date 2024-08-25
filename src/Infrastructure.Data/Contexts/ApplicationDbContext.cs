using System.Reflection;
using CleanArch.Domain.AuditEventProcessor;
using CleanArch.Infrastructure.Data.Converters;
using CleanArch.Infrastructure.Data.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Data.Contexts;

public class ApplicationDbContext(
    DataSecurityService dataSecurityService,
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AuditEventRecord> AuditEventRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        AddProtectedPersonDataConverter(modelBuilder);
    }

    private void AddProtectedPersonDataConverter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType
                .GetProperties()
                .Where(p =>
                    p.PropertyType == typeof(string) &&
                    p.GetCustomAttribute<ProtectedPersonalDataAttribute>() != null);

            foreach (var property in properties)
            {
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(new ProtectedPersonDataConverter(dataSecurityService));
            }
        }
    }
}
