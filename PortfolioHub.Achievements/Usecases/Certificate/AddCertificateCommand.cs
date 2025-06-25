using Ardalis.Result;
using MediatR;
using PortfolioHub.Achievements.Domain;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed record AddCertificateCommand(
    string Name,
    string Issuer,
    DateTime Date
    ) : IRequest<Result<Guid>>;
