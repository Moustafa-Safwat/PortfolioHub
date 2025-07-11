using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.ProfessionalSkills;

internal sealed class DeleteProfessionalSkillCommandHandler(
    IEntityRepo<Domain.Entities.ProfessionalSkill> professionalSkillRepo
    ) : IRequestHandler<DeleteProfessionalSkillCommand, Result>
{
    public async Task<Result> Handle(DeleteProfessionalSkillCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await professionalSkillRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
            return deleteResult;

        var effectedRowsResult = await professionalSkillRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
            return effectedRowsResult;

        return Result.SuccessWithMessage("The professional skill entry was deleted successfully.");
    }
}