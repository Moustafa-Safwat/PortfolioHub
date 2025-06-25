using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed record DeleteProfessionalSkillCommand(Guid Id) : IRequest<Result>;

internal sealed class DeleteProfessionalSkillCommandHandler(
    IEntityRepo<Domain.Entities.ProfessionalSkill> professionalSkillRepo,
    ILogger logger
    ) : IRequestHandler<DeleteProfessionalSkillCommand, Result>
{
    public async Task<Result> Handle(DeleteProfessionalSkillCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to delete ProfessionalSkill with Id: {ProfessionalSkillId}", request.Id);

        var deleteResult = await professionalSkillRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
        {
            logger.Warning("Failed to delete ProfessionalSkill with Id: {ProfessionalSkillId}. Reason: {Reason}", request.Id, deleteResult.Errors?.FirstOrDefault() ?? "Unknown error");
            return deleteResult;
        }

        var effectedRowsResult = await professionalSkillRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
        {
            logger.Warning("Failed to save changes after deleting ProfessionalSkill with Id: {ProfessionalSkillId}. Reason: {Reason}", request.Id, effectedRowsResult.Errors?.FirstOrDefault() ?? "Unknown error");
            return effectedRowsResult;
        }

        logger.Information("Successfully deleted ProfessionalSkill with Id: {ProfessionalSkillId}", request.Id);
        return Result.SuccessWithMessage("The professional skill entry was deleted successfully.");
    }
}