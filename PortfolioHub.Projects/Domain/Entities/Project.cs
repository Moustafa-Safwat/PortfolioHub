using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain.Entities;

internal class Project: BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string VideoUrl { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }

    // Navigation property
    public ICollection<Gallery>? Images { get; private set; } = new List<Gallery>();
    public ICollection<TechanicalSkills>? Skills { get; private set; } = new List<TechanicalSkills>();
    public ICollection<Links>? Links { get; private set; } = new List<Links>();


    // Parameterless constructor for EF Core
    private Project() { }

    public Project(Guid id, string title, string description, string videoUrl, DateTime createdDate)
    {
        Id = Guard.Against.Default(id);
        Title = Guard.Against.NullOrEmpty(title);
        Description = Guard.Against.NullOrEmpty(description);
        VideoUrl = Guard.Against.Null(videoUrl);
        CreatedDate = Guard.Against.OutOfSQLDateRange(createdDate);
    }

    public void SetTitle(string title)
    {
        Title = Guard.Against.NullOrEmpty(title);
    }

    public void SetDescription(string description)
    {
        Description = Guard.Against.NullOrEmpty(description);
    }

    public void SetVideoUrl(string videoUrl)
    {
        VideoUrl = Guard.Against.Null(videoUrl);
    }

    public void SetCreatedDate(DateTime createdDate)
    {
        CreatedDate = Guard.Against.OutOfSQLDateRange(createdDate);
    }

}
