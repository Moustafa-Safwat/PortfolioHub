using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed record DeleteProfessionalSkillCommand(Guid Id) : IRequest<Result>;
