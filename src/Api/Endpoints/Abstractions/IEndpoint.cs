namespace CleanArch.Api.Endpoints.Abstractions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder builder);
}
