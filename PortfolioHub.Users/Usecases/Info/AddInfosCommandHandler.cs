using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed class AddInfosCommandHandler(
    IInfoRepo infoRepo
    ) : IRequestHandler<AddInfosCommand, Result<Guid[]>>
{
    public async Task<Result<Guid[]>> Handle(AddInfosCommand request, CancellationToken cancellationToken)
    {
        var infoEntities = request.Infos
            .Select(info => new Domain.Entities.Info
                (
                    Guid.NewGuid(),
                    info.InfoKey,
                    info.InfoValue
                )
            ).ToList();

        var result = await infoRepo.AddRangeAsync(infoEntities, cancellationToken);

        return result;
    }
}