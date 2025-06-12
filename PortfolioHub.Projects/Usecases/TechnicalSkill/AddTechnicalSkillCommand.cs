using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed record AddTechnicalSkillCommand(
    string Name
    ) : IRequest<Result<Guid>>;
