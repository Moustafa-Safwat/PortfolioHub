using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed record AddProfessionalSkillCommand(string name)
    : IRequest<Result<Guid>>;
