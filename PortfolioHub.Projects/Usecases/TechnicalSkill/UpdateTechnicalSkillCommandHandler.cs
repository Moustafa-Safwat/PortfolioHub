using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class UpdateTechnicalSkillCommandHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo
    ) : IRequestHandler<UpdateTechnicalSkillCommand, Result>
{
    public async Task<Result> Handle(UpdateTechnicalSkillCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out Guid Id))
            return Result.Error("Invalid ID format. Please provide a valid GUID.");

        var techSkillEntity = new TechanicalSkills(Id, request.Name);

        var updateResult = await techSkillRepo.UpdateAsync(techSkillEntity, cancellationToken);

        var saveResult = await techSkillRepo.SaveChangesAsync(cancellationToken);

        if (!(updateResult.IsSuccess && saveResult.IsSuccess))
            return Result.Error($"Faild to update skill with id {Id}");

        return Result.SuccessWithMessage($"Skill with id {Id} updated successfully.");
    }
}
