using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain.Entities;

internal class Links : BaseEntity
{
    public string Url { get; private set; } = string.Empty;
    // Navigation property
    public Project? Project { get; private set; } = null!;
    public LinkProvider LinkProvider { get; private set; } = null!;
    // Parameterless constructor for EF Core
    private Links() { }
    public Links(Guid id, string url)
    {
        Id = Guard.Against.Default(id);
        Url = Guard.Against.NullOrEmpty(url);
    }

    public Links SetLinkProvider(LinkProvider linkProvider)
    {
        LinkProvider = Guard.Against.Null(linkProvider);
        return this;
    }
}
