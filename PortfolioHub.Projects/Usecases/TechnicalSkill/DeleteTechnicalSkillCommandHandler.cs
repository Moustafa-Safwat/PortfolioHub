using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class DeleteTechnicalSkillCommandHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo,
    ILogger logger
    ) : IRequestHandler<DeleteTechnicalSkillCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(DeleteTechnicalSkillCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to delete TechnicalSkill with Id: {TechnicalSkillId}", request.Id);

        var deleteResult = await techSkillRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
        {
            logger.Warning("Failed to delete TechnicalSkill with Id: {TechnicalSkillId}. Errors: {Errors}", request.Id, string.Join(", ", deleteResult.Errors));
            return Result<Guid>.Error(new ErrorList(deleteResult.Errors));
        }

        var saveResult = await techSkillRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("Failed to save changes after deleting TechnicalSkill with Id: {TechnicalSkillId}. Errors: {Errors}", request.Id, string.Join(", ", saveResult.Errors));
            return Result<Guid>.Error(new ErrorList(saveResult.Errors));
        }

        logger.Information("Successfully deleted TechnicalSkill with Id: {TechnicalSkillId}", request.Id);
        return Result.Success(request.Id);
    }
}