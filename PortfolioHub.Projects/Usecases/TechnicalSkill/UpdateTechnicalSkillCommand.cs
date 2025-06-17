using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed record UpdateTechnicalSkillCommand(
    string Id,
    string Name) : IRequest<Result>;
