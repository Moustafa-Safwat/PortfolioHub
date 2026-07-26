using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostLike : BaseEntity
{
    public Guid BlogPostId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime LikedAt { get; private set; }
    public bool IsLiked { get; private set; }
    public DateTime UnLikedAt { get; private set; }
    // Navigation property
    public BlogPost BlogPost { get; private set; } = null!;
    // EF
    private BlogPostLike() { }
    public BlogPostLike(
        Guid blogPostId,
        Guid userId)
    {
        Id = Guid.NewGuid();
        BlogPostId = Guard.Against.Default(blogPostId);
        UserId = Guard.Against.Default(userId);
        LikedAt = DateTime.UtcNow;
        IsLiked = true;
    }
    // Methods
    public void UnLike()
    {
        IsLiked = false;
        UnLikedAt = DateTime.UtcNow;
    }
}
