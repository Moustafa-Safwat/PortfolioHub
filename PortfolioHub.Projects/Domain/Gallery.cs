using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain;

internal class Gallery
{
    public Guid Id { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public int Order { get; private set; }

    // Navigation property
    public Project Project { get; private set; } = null!;

    // Parameterless constructor for EF Core
    private Gallery() { }

    public Gallery(Guid id, string imageUrl, int order)
    {
        Id = Guard.Against.Default(id);
        ImageUrl = Guard.Against.NullOrEmpty(imageUrl);
        Order = Guard.Against.Negative(order);
    }
}
