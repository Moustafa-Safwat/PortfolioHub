using Ardalis.GuardClauses;

namespace PortfolioHub.Projects.Domain.Entities;

internal class Project : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string LongDescription { get; private set; } = string.Empty;
    public string VideoUrl { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }
    public string CoverImageUrl { get; private set; } = string.Empty;

    // Navigation property
    public ICollection<Gallery>? Images { get; private set; } = new List<Gallery>();
    public ICollection<TechanicalSkills>? Skills { get; private set; } = new List<TechanicalSkills>();
    public ICollection<Links>? Links { get; private set; } = new List<Links>();
    public Category? Category { get; private set; } = new Category();


    // Parameterless constructor for EF Core
    private Project() { }

    public Project(Guid id, string title, string description,
        string longDescription, string videoUrl, DateTime createdDate,
        string coverImageUrl)
    {
        Id = Guard.Against.Default(id);
        Title = Guard.Against.NullOrEmpty(title);
        Description = Guard.Against.NullOrEmpty(description);
        LongDescription = Guard.Against.NullOrEmpty(longDescription);
        VideoUrl = Guard.Against.Null(videoUrl);
        CreatedDate = Guard.Against.OutOfSQLDateRange(createdDate);
        CoverImageUrl = Guard.Against.NullOrEmpty(coverImageUrl);
    }

    public void SetTitle(string title)
        => Title = Guard.Against.NullOrEmpty(title);

    public void SetDescription(string description)
        => Description = Guard.Against.NullOrEmpty(description);

    public void SetLongDescription(string longDescription)
        => LongDescription = Guard.Against.NullOrEmpty(longDescription);

    public void SetCoverImageUrl(string coverImageUrl)
        => CoverImageUrl = Guard.Against.NullOrEmpty(coverImageUrl);

    public void SetVideoUrl(string videoUrl)
        => VideoUrl = Guard.Against.Null(videoUrl);

    public void SetCreatedDate(DateTime createdDate)
        => CreatedDate = Guard.Against.OutOfSQLDateRange(createdDate);

    public void SetTechanicalSkills(IList<TechanicalSkills> techanicalSkills)
        => Skills = Guard.Against.NullOrEmpty(techanicalSkills).ToList();

    public void SetImages(IList<Gallery> images)
        => Images = Guard.Against.NullOrEmpty(images).ToList();

    public void SetLinks(IList<Links> links)
        => Links = Guard.Against.NullOrEmpty(links).ToList();

    public void SetCategory(Category category)
        => Category = Guard.Against.Null(category);
}
