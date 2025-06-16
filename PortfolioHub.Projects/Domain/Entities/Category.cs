using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Projects.Domain.Entities;

internal class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    // Parameterless constructor for EF Core
    public Category() { }

    public Category(Guid id, string name)
    {
        Id = Guard.Against.Default(id);
        Name = Guard.Against.NullOrEmpty(name);
    }

}
