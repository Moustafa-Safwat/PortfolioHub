using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Endpoints.TechanicalSkills;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed record GetTechnicalSkillQuery
    : IRequest<Result<IEnumerable<TechSkillDto>>>;
