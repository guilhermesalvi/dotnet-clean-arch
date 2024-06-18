using System.Reflection;
using Asp.Versioning;
using CleanArch.Api.Endpoints.Abstractions;

namespace CleanArch.Api.Extensions;

public static class EndpointExtensions
{
    public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
    {
        var builder = (WebApplication)app;

        var versionSet = builder
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var router = builder
            .MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1, 0);

        Assembly.GetExecutingAssembly().DefinedTypes
            .Where(x => x.ImplementedInterfaces.Contains(typeof(IEndpoint)))
            .ToList()
            .ForEach(x => x.GetMethod("MapEndpoint")?.Invoke(Activator.CreateInstance(x), [router]));

        return app;
    }
}
