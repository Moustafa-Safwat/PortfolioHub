namespace PortfolioHub.Achievements.Endpoints.Education;

internal sealed record EducationGetDto(
    Guid Id,
    string Degree,
    string Institution,
    string FieldOfStudy,
    string Description,
    DateTime StartDate,
    DateTime? EndDate
);
