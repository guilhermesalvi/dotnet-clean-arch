using System.Security.Cryptography;
using CleanArch.Api.Extensions;
using CleanArch.Application.Extensions;
using CleanArch.Infrastructure.Data.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container
    builder.Services
        .AddVersioning()
        .AddNotifications()
        .AddEndpointsApiExplorer()
        .AddDocumentation()
        .AddGlobalExceptionHandler()
        .AddApplication(builder.Configuration)
        .AddData(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline
    app
        .UseNotifications()
        .UseGlobalExceptionHandler()
        .UseHttpsRedirection()
        .UseRouting()
        .UseEndpoints()
        .UseDocumentation();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

public abstract partial class Program;
