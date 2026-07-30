using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostAuthor : DeletionEntity
{
    public Guid BlogPostId { get; private set; }
    public Guid UserId { get; private set; }
    public BlogPostAuthorRole Role { get; private set; }
    // Navigation properties
    public BlogPost BlogPost { get; private set; } = null!;
    // EF
    private BlogPostAuthor() { }
    public BlogPostAuthor(
        Guid blogPostId,
        Guid userId,
        BlogPostAuthorRole role,
        Guid addedByUserId)
    {
        Id = Guid.NewGuid();
        BlogPostId = Guard.Against.Default(blogPostId);
        UserId = Guard.Against.Default(userId);
        Role = Guard.Against.EnumOutOfRange(role);
        MarkAsCreated(addedByUserId);
    }
    // Methods
    public void ChangeRole(BlogPostAuthorRole newRole, Guid updatedByUserId)
    {
        Role = Guard.Against.EnumOutOfRange(newRole);
        MarkAsUpdated(updatedByUserId);
    }
    public void RemoveAuthor(Guid removedByUserId)
    {
        MarkAsDeleted(removedByUserId);
    }
}
