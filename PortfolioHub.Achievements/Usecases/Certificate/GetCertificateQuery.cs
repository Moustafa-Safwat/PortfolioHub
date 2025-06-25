using Ardalis.Result;
using MediatR;
using PortfolioHub.Achievements.Endpoints.Certificate;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed record GetCertificateQuery
    (
        int Page,
        int PageSize
    ) : IRequest<Result<IEnumerable<CertificateGetDto>>>;
