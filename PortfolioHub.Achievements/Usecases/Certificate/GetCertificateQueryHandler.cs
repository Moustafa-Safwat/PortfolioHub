using Ardalis.Result;
using MediatR;
using PortfolioHub.Achievements.Endpoints.Certificate;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed class GetCertificateQueryHandler
    (
        IEntityRepo<Domain.Certificate> certificateRepo,
        ILogger logger
    ) : IRequestHandler<GetCertificateQuery, Result<IEnumerable<CertificateGetDto>>>
{
    public async Task<Result<IEnumerable<CertificateGetDto>>> Handle(GetCertificateQuery request, CancellationToken cancellationToken)
    {
        logger.Information("Handling GetCertificateQuery: Page={Page}, PageSize={PageSize}", request.Page, request.PageSize);

        var result = await certificateRepo.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        if (!result.IsSuccess)
        {
            logger.Warning("Failed to retrieve certificates. ValidationErrors: {@ValidationErrors}", result.ValidationErrors);
            return Result.Invalid(result.ValidationErrors);
        }

        var certificateGetDto = result.Value.Select(
            c => new CertificateGetDto(
                c.Id,
                c.Name,
                c.Issuer,
                c.Date
            )
        );

        logger.Information("Successfully retrieved {Count} certificates.", certificateGetDto.Count());

        return Result<IEnumerable<CertificateGetDto>>.Success(certificateGetDto);
    }
}