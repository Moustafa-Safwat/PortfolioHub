using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Achievements.Usecases.Education;

internal sealed class DeleteEducationCommandHandler(
    IEntityRepo<Domain.Education> educationRepo
) : IRequestHandler<DeleteEducationCommand, Result>
{
    public async Task<Result> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
    {
         await educationRepo.DeleteAsync(request.Id, cancellationToken);
        var effectedRows = await educationRepo.SaveChangesAsync(cancellationToken);

        if (!effectedRows.IsSuccess)
            return Result.Invalid(effectedRows.ValidationErrors);

        return Result.SuccessWithMessage("The education entry was deleted successfully.");
    }
}
