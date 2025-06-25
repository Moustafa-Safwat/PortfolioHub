using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed class AddProfessionalSkillCommandHandler(
    IEntityRepo<Domain.Entities.ProfessionalSkill> repository,
    ILogger logger
    ) : IRequestHandler<AddProfessionalSkillCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(AddProfessionalSkillCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Handling AddProfessionalSkillCommand for skill name: {SkillName}", request.name);

        var skill = new Domain.Entities.ProfessionalSkill(Guid.NewGuid(), request.name);

        await repository.AddAsync(skill, cancellationToken);
        var effectedRowsResult = await repository.SaveChangesAsync(cancellationToken);

        if (!effectedRowsResult.IsSuccess)
        {
            logger.Error("Failed to add ProfessionalSkill '{SkillName}'. Errors: {Errors}", request.name, effectedRowsResult.Errors);
            return Result<Guid>.Error(new ErrorList(effectedRowsResult.Errors));
        }

        logger.Information("Successfully added ProfessionalSkill '{SkillName}' with Id: {SkillId}", request.name, skill.Id);
        return Result<Guid>.Success(skill.Id);
    }
}