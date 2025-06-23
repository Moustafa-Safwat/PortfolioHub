using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed class DeleteInfoCommandHandler(
    IInfoRepo infoRepo,
    ILogger logger
    ) : IRequestHandler<DeleteInfoCommand, Result>
{
    public async Task<Result> Handle(DeleteInfoCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to delete Info with key: {Key}", request.key);

        var result = await infoRepo.DeleteRangeAsync([request.key], cancellationToken);

        if (!result.IsSuccess)
        {
            logger.Warning("Failed to delete Info with key: {Key}. Status: {Status}. Errors: {Errors}",
                request.key, result.Status, string.Join(", ", result.Errors));

            switch (result.Status)
            {
                case ResultStatus.Invalid:
                    return Result.Invalid(result.ValidationErrors);
                case ResultStatus.NotFound:
                    return Result.NotFound(result.Errors.ToArray());
            }
        }

        logger.Information("Successfully deleted Info with key: {Key}", request.key);
        return Result.Success();
    }
}