using Ardalis.Result;
using MediatR;
using PortfolioHub.Achievements.Endpoints.Certificate;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed class GetCertificateQueryHandler
    (
        IEntityRepo<Domain.Certificate> certificateRepo
    ) : IRequestHandler<GetCertificateQuery, Result<IEnumerable<CertificateGetDto>>>
{
    public async Task<Result<IEnumerable<CertificateGetDto>>> Handle(GetCertificateQuery request, CancellationToken cancellationToken)
    {
        var result = await certificateRepo.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        if (!result.IsSuccess)
            return Result.Invalid(result.ValidationErrors);

        var certificateGetDto = result.Value.Select(
            c => new CertificateGetDto(
                c.Id,
                c.Name,
                c.Issuer,
                c.Date
            )
        );

        return Result<IEnumerable<CertificateGetDto>>.Success(certificateGetDto);
    }
}