using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed record DeleteTechnicalSkillCommand(
    Guid Id
    ) : IRequest<Result<Guid>>;
