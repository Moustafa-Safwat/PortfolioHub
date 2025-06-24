using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Achievements.Domain;

internal sealed class Education : BaseEntity
{
    public string Degree { get; private set; } = string.Empty;
    public string Institution { get; private set; } = string.Empty;
    public string FieldOfStudy { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; } = null;

    public Education(Guid id, string degree, string institution, string fieldOfStudy,
    string description, DateTime startDate, DateTime? endDate)
    {
        Id = Guard.Against.Default(id);
        Degree = Guard.Against.NullOrEmpty(degree);
        Institution = Guard.Against.NullOrEmpty(institution);
        FieldOfStudy = Guard.Against.NullOrEmpty(fieldOfStudy);
        Description = Guard.Against.NullOrEmpty(description);
        StartDate = Guard.Against.OutOfSQLDateRange(startDate);
        EndDate = endDate; // Nullable, so no Guard clause needed here
    }

    public void MarkAsOngoing() => EndDate = null;
}
