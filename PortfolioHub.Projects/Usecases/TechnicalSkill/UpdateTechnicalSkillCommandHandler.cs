using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class UpdateTechnicalSkillCommandHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo,
    ILogger logger
    ) : IRequestHandler<UpdateTechnicalSkillCommand, Result>
{
    public async Task<Result> Handle(UpdateTechnicalSkillCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Handling UpdateTechnicalSkillCommand for Id: {Id}, Name: {Name}", request.Id, request.Name);

        if (!Guid.TryParse(request.Id, out Guid Id))
        {
            logger.Warning("Invalid ID format received: {Id}", request.Id);
            return Result.Error("Invalid ID format. Please provide a valid GUID.");
        }

        var techSkillEntity = new TechanicalSkills(Id, request.Name);
        logger.Debug("Created TechanicalSkills entity for update: {@TechSkillEntity}", techSkillEntity);

        var updateResult = await techSkillRepo.UpdateAsync(techSkillEntity, cancellationToken);
        logger.Debug("UpdateAsync result: {@UpdateResult}", updateResult);

        var saveResult = await techSkillRepo.SaveChangesAsync(cancellationToken);
        logger.Debug("SaveChangesAsync result: {@SaveResult}", saveResult);

        if (!(updateResult.IsSuccess && saveResult.IsSuccess))
        {
            logger.Error("Failed to update skill with id {Id}. UpdateResult: {@UpdateResult}, SaveResult: {@SaveResult}", Id, updateResult, saveResult);
            return Result.Error($"Faild to update skill with id {Id}");
        }

        logger.Information("Skill with id {Id} updated successfully.", Id);
        return Result.SuccessWithMessage($"Skill with id {Id} updated successfully.");
    }
}
