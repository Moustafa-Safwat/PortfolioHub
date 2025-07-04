using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace PortfolioHub.Web.Infra.Crosscutting;

/// <summary>
/// Pipeline behavior for logging MediatR requests and responses, including validation errors.
/// </summary>
internal class LoggingPipeLineBehaviour<TRequest, TResponse>(
    ILogger<LoggingPipeLineBehaviour<TRequest, TResponse>> logger
    )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling request {RequestType} at {DateTimeUtc} with data: {@Request}",
            typeof(TRequest).Name, DateTime.UtcNow, request);

        try
        {
            var response = await next();

            logger.LogInformation("Request {RequestType} completed at {DateTimeUtc} with response: {@Response}",
                typeof(TRequest).Name, DateTime.UtcNow, response);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request {RequestType} threw an exception at {DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);
            throw;
        }
    }
}
