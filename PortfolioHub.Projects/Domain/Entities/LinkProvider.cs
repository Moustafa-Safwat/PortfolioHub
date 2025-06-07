using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain.Entities;

internal class LinkProvider: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string BaseUrl { get; private set; } = string.Empty;
    // Navigation property
    public ICollection<Links>? Links { get; private set; } = new List<Links>();
    // Parameterless constructor for EF Core
    private LinkProvider() { }
    public LinkProvider(Guid id, string name, string baseUrl)
    {
        Id = Guard.Against.Default(id);
        Name = Guard.Against.NullOrEmpty(name);
        BaseUrl = Guard.Against.NullOrEmpty(baseUrl);
    }
}
