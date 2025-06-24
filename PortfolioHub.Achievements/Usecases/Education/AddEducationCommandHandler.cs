using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Achievements.Usecases.Education;

internal sealed class AddEducationCommandHandler
    (
        IEntityRepo<Domain.Education> educationRepo
    ) : IRequestHandler<AddEducationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddEducationCommand request, CancellationToken cancellationToken)
    {
        var education = new Domain.Education(
            Guid.NewGuid(),
            request.Degree,
            request.Institution,
            request.FieldOfStudy,
            request.Description,
            request.StartDate,
            request.EndDate);

        await educationRepo.AddAsync(education, cancellationToken);
        var effectedRowsResult = await educationRepo.SaveChangesAsync(cancellationToken);

        if (!effectedRowsResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(effectedRowsResult.Errors));

        return Result<Guid>.Success(education.Id);
    }
}