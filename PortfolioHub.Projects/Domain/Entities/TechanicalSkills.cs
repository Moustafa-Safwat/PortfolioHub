using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain.Entities;

internal class TechanicalSkills: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    // Navigation property
    public ICollection<Project>? Projects { get; private set; } = new List<Project>();

    // Parameterless constructor for EF Core
    public TechanicalSkills() { }

    public TechanicalSkills(Guid id, string name)
    {
        Id = Guard.Against.Default(id);
        Name = Guard.Against.NullOrEmpty(name);
    }
}
