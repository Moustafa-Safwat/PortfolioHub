using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Endpoints.ProfessionalSkills;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed class GetProfessionalSkillsQueryHandler(
    IEntityRepo<Domain.Entities.ProfessionalSkill> professionalSkillRepo
) : IRequestHandler<GetProfessionalSkillsQuery, Result<IEnumerable<ProfessionalSkillGetDto>>>
{
    public async Task<Result<IEnumerable<ProfessionalSkillGetDto>>> Handle(GetProfessionalSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var professionalSkills = await professionalSkillRepo.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        if (!professionalSkills.IsSuccess)
            return Result.Invalid(professionalSkills.ValidationErrors);

        var professionalSkillDtos = professionalSkills.Value
            .Select(ps => new ProfessionalSkillGetDto(
                ps.Id,
                ps.Name
            ))
            .ToList();

        return Result<IEnumerable<ProfessionalSkillGetDto>>.Success(professionalSkillDtos);
    }
}