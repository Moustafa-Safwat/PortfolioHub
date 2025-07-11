using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.TechnicalSkill;

internal sealed class DeleteTechnicalSkillCommandHandler(
    IEntityRepo<TechanicalSkills> techSkillRepo
    ) : IRequestHandler<DeleteTechnicalSkillCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(DeleteTechnicalSkillCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await techSkillRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(deleteResult.Errors));

        var saveResult = await techSkillRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(saveResult.Errors));

        return Result.Success(request.Id);
    }
}