using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArch.Api.Extensions;

public static class DocumentationExtensions
{
    private const string AppName = "CleanArch";

    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        return services
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddSwaggerGen();
    }

    public static IApplicationBuilder UseDocumentation(this IApplicationBuilder app)
    {
        return app
            .UseStaticFiles()
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                var groupNames = ((IEndpointRouteBuilder)app).DescribeApiVersions();

                foreach (var groupName in groupNames.Select(x => x.GroupName))
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{groupName}/swagger.json",
                        $"{AppName} {groupName}");
                    options.RoutePrefix = "docs";
                }

                options.InjectStylesheet("/swagger-ui/swagger-dark.css");
            });
    }

    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) :
        IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = $"{AppName} {description.GroupName}",
                Description = description.IsDeprecated
                    ? $"{AppName} - DEPRECATED"
                    : $"{AppName}"
            };

            return info;
        }
    }
}
