using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostTag : BaseEntity
{
    public string Name { get; private set; } = null!;
    // Navigation property
    public ICollection<BlogPost> BlogPosts { get; private set; } = new List<BlogPost>();
    // EF Constructor
    private BlogPostTag() { }
    public BlogPostTag(string name)
    {
        Id = Guid.NewGuid();
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}
