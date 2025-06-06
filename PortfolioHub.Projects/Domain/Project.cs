using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain;

internal class Project
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string VideoUrl { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }

    // Navigation property
    public ICollection<Gallery> Images { get; private set; } = new List<Gallery>();
    public ICollection<TechanicalSkills> Skills { get; private set; } = new List<TechanicalSkills>();
    public ICollection<Links> Links { get; private set; } = new List<Links>();


    // Parameterless constructor for EF Core
    private Project() { }

    public Project(Guid id, string title, string description, string videoUrl, DateTime createdDate)
    {
        Id = Guard.Against.Default(id);
        Title = Guard.Against.NullOrEmpty(title);
        Description = Guard.Against.NullOrEmpty(description);
        VideoUrl = Guard.Against.NullOrEmpty(videoUrl);
        CreatedDate = Guard.Against.OutOfSQLDateRange(createdDate);
    }
}
