using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Endpoints.Info;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed class GetInfoByKeysQueryHandler(
    IInfoRepo infoRepository
    ) : IRequestHandler<GetInfoByKeysQuery, Result<IEnumerable<InfoGetDto>>>
{
    public async Task<Result<IEnumerable<InfoGetDto>>> Handle(GetInfoByKeysQuery request, CancellationToken cancellationToken)
    {
        var infosResult = await infoRepository.GetByKeysAsync(request.Keys, cancellationToken);
        if (!infosResult.IsSuccess)
            return Result.Invalid(infosResult.ValidationErrors);

        var infos = infosResult.Value;
        if (infos == null || !infos.Any())
            return Result.NotFound(["No information found for the provided keys."]);

        var infoDtos = infos.Select(info => new InfoGetDto(info.Id, info.InfoKey, info.InfoValue));
        return Result.Success(infoDtos);
    }
}
