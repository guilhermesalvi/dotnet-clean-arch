using CleanArch.Application.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

#pragma warning disable CS8603 // Possible null reference return.

namespace CleanArch.Application.RequestValidation;

public class RequestValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    NotificationManager manager,
    ILogger<RequestValidationPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = validators
            .Select(async x =>
                await x.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken))
            .Select(x => x.Result)
            .Where(x => !x.IsValid)
            .ToList();

        if (failures.Count == 0) return await next();

        foreach (var error in failures.SelectMany(failure => failure.Errors))
        {
            manager.AddNotification(error.ErrorCode, error.CustomState);
        }

        logger.LogWarning("{Request} not sent to handler because validation failed", typeof(TRequest).Name);
        return default;
    }
}
