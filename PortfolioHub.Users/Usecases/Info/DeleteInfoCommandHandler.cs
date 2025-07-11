using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed class DeleteInfoCommandHandler(
    IInfoRepo infoRepo
    ) : IRequestHandler<DeleteInfoCommand, Result>
{
    public async Task<Result> Handle(DeleteInfoCommand request, CancellationToken cancellationToken)
    {
        var result = await infoRepo.DeleteRangeAsync([request.key], cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.Status)
            {
                case ResultStatus.Invalid:
                    return Result.Invalid(result.ValidationErrors);
                case ResultStatus.NotFound:
                    return Result.NotFound(result.Errors.ToArray());
            }
        }

        return Result.Success();
    }
}