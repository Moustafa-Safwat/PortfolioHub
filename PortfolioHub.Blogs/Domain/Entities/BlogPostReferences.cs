using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostReferences : DeletionEntity
{
    public Guid BlogPostId { get; private set; }
    public string Url { get; private set; } = null!;
    public string Label { get; private set; } = null!;
    // Navigation property
    public BlogPost BlogPost { get; private set; } = null!;
    // EF Constructor
    private BlogPostReferences() { }
    public BlogPostReferences(Guid blogPostId, Guid userId, string url, string label)
    {
        Id = Guid.NewGuid();
        BlogPostId = Guard.Against.Default(blogPostId);
        SetUrl(url);
        SetLabel(label);
        MarkAsCreated(userId);
    }
    // Methods
    public void Delete(Guid userId)
        => MarkAsDeleted(userId);

    public void Update(string url, string label, Guid userId)
    {
        Url = Guard.Against.NullOrWhiteSpace(url);
        Label = Guard.Against.NullOrWhiteSpace(label);
        MarkAsUpdated(userId);
    }

    public void SetUrl(string url)
        => Url = Guard.Against.NullOrWhiteSpace(url);

    public void SetLabel(string label)
        => Label = Guard.Against.NullOrWhiteSpace(label);

}
