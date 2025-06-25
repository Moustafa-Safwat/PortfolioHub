using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Endpoints.ProfessionalSkills;
using Serilog;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed class GetProfessionalSkillsQueryHandler(
    IEntityRepo<Domain.Entities.ProfessionalSkill> professionalSkillRepo,
    ILogger logger
) : IRequestHandler<GetProfessionalSkillsQuery, Result<IEnumerable<ProfessionalSkillGetDto>>>
{
    public async Task<Result<IEnumerable<ProfessionalSkillGetDto>>> Handle(GetProfessionalSkillsQuery request,
        CancellationToken cancellationToken)
    {
        logger.Information("Handling GetProfessionalSkillsQuery: Page={Page}, PageSize={PageSize}", request.Page, request.PageSize);

        var professionalSkills = await professionalSkillRepo.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        if (!professionalSkills.IsSuccess)
        {
            logger.Warning("Failed to retrieve professional skills. ValidationErrors: {@ValidationErrors}", professionalSkills.ValidationErrors);
            return Result.Invalid(professionalSkills.ValidationErrors);
        }

        var professionalSkillDtos = professionalSkills.Value
            .Select(ps => new ProfessionalSkillGetDto(
                ps.Id,
                ps.Name
            ))
            .ToList();

        logger.Information("Successfully retrieved {Count} professional skills.", professionalSkillDtos.Count);

        return Result<IEnumerable<ProfessionalSkillGetDto>>.Success(professionalSkillDtos);
    }
}