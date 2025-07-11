using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Endpoints.TechanicalSkills;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class GetTechnicalSkillQueryHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo
    ) : IRequestHandler<GetTechnicalSkillQuery, Result<IEnumerable<TechSkillDto>>>
{
    public async Task<Result<IEnumerable<TechSkillDto>>> Handle(
        GetTechnicalSkillQuery request,
        CancellationToken cancellationToken)
    {
        var getTechSkillResult = await techSkillRepo.GetAllAsync(1, int.MaxValue, cancellationToken);
        if (!getTechSkillResult.IsSuccess)
            return Result<IEnumerable<TechSkillDto>>.Error(new ErrorList(getTechSkillResult.Errors?.ToArray() ?? ["Failed to retrieve technical skills."]));

        var techSkill = getTechSkillResult.Value;
        if (techSkill == null)
            return Result<IEnumerable<TechSkillDto>>.Error();

        var dto = techSkill.Select(tech => new TechSkillDto(tech.Id, tech.Name));

        return Result.Success(dto);
    }
}