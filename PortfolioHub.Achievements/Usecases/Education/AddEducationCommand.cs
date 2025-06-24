using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Achievements.Usecases.Education;

internal sealed record AddEducationCommand
    (
        string Degree,
        string Institution,
        string FieldOfStudy,
        string Description,
        DateTime StartDate,
        DateTime? EndDate
    ) : IRequest<Result<Guid>>;
