using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostView : BaseEntity
{
    public Guid BlogPostId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ViewAt { get; private set; }
    public DateTime? LastViewAt { get; private set; }
    public int ViewCount { get; private set; }
    // Navigation property
    public BlogPost BlogPost { get; private set; } = null!;
    // EF Constructor
    private BlogPostView() { }
    public BlogPostView(
        Guid blogPostId,
        Guid userId,
        DateTime viewedAtUtc)
    {
        Id = Guid.NewGuid();
        BlogPostId = Guard.Against.Default(blogPostId);
        UserId = Guard.Against.Default(userId);
        ViewAt = viewedAtUtc;
        ViewCount = 1;
    }
    // Methods
    public void IncrementViewCount()
    {
        ViewCount++;
        LastViewAt = DateTime.UtcNow;
    }
}
