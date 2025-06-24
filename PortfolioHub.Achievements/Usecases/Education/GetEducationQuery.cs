using Ardalis.Result;
using MediatR;
using PortfolioHub.Achievements.Endpoints.Education;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Achievements.Usecases.Education;

internal sealed record GetEducationQuery(
    int Page,
    int PageSize
    ) : IRequest<Result<IEnumerable<EducationGetDto>>>;

internal sealed class GetEducationQueryHandler(
    IEntityRepo<Domain.Education> educationRepo
    ) : IRequestHandler<GetEducationQuery, Result<IEnumerable<EducationGetDto>>>
{
    public async Task<Result<IEnumerable<EducationGetDto>>> Handle(GetEducationQuery request,
        CancellationToken cancellationToken)
    {
        var educations = await educationRepo.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        if (!educations.IsSuccess)
            return Result.Invalid(educations.ValidationErrors);

        var educationDtos = educations.Value
            .Select(e => new EducationGetDto(
                e.Id,
                e.Degree,
                e.Institution,
                e.FieldOfStudy,
                e.Description,
                e.StartDate,
                e.EndDate
            ))
            .ToList();

        return Result<IEnumerable<EducationGetDto>>.Success(educationDtos);
    }
}