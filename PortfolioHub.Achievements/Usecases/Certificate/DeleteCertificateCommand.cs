using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed record DeleteCertificateCommand(Guid Id)
    : IRequest<Result>;
