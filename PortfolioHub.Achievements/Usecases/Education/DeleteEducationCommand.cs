using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Achievements.Usecases.Education;

internal sealed record DeleteEducationCommand(Guid Id)
    : IRequest<Result>;
