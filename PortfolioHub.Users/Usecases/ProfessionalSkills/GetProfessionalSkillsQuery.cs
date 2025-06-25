using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Endpoints.ProfessionalSkills;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed record GetProfessionalSkillsQuery(int Page, int PageSize) : IRequest<Result<IEnumerable<ProfessionalSkillGetDto>>>;
