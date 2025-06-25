using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Users.Domain.Entities;

internal sealed class ProfessionalSkill : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public ProfessionalSkill(Guid id, string name)
    {
        Id = Guard.Against.Default(id);
        Name = Guard.Against.NullOrEmpty(name);
    }
}
