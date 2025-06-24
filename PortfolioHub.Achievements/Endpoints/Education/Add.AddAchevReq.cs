namespace PortfolioHub.Achievements.Endpoints.Education;

internal sealed record AddAchevReq(
    string Degree,
    string Institution,
    string FieldOfStudy,
    string Description,
    DateTime StartDate,
    DateTime? EndDate);
