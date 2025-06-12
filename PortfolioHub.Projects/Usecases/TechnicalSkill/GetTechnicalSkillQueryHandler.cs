using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.TechanicalSkills;
using Serilog;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class GetTechnicalSkillQueryHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo,
    ILogger logger
    ) : IRequestHandler<GetTechnicalSkillQuery, Result<IEnumerable<TechSkillDto>>>
{
    public async Task<Result<IEnumerable<TechSkillDto>>> Handle(
        GetTechnicalSkillQuery request,
        CancellationToken cancellationToken)
    {
        var getTechSkillResult = await techSkillRepo.GetAllAsync(1, int.MaxValue, cancellationToken);
        if (!getTechSkillResult.IsSuccess)
        {
            logger.Error("Failed to retrieve technical skills: {ErrorMessage}", getTechSkillResult.Errors?.FirstOrDefault() ?? "Unknown error");
            return Result<IEnumerable<TechSkillDto>>.Error(new ErrorList(getTechSkillResult.Errors?.ToArray() ?? ["Failed to retrieve technical skills."]));
        }

        var techSkill = getTechSkillResult.Value;
        if (techSkill == null)
        {
            logger.Warning("No technical skills found in the repository.");
            return Result<IEnumerable<TechSkillDto>>.Error();
        }

        var dto = techSkill.Select(tech => new TechSkillDto(tech.Id, tech.Name));

        logger.Information("Successfully retrieved {Count} technical skills. Ids: {Ids}", 
            techSkill.Count(), 
            string.Join(", ", techSkill.Select(t => t.Id)));

        return Result.Success(dto);
    }
}