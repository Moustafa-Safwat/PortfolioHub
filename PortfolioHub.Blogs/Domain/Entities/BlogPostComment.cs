using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPostComment : DeletionEntity
{
    private readonly IList<BlogPostComment> _replies = [];
    private readonly IList<Guid> _mentions = [];

    public Guid BlogPostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = null!;
    public IReadOnlyCollection<BlogPostComment> Replies => _replies.AsReadOnly();
    public IReadOnlyCollection<Guid> Mentions => _mentions.AsReadOnly();
    // Navigation properties
    public BlogPost BlogPost { get; private set; } = null!;

    // EF Constructor
    private BlogPostComment() { }

    public BlogPostComment(Guid blogPostId, Guid userId, string content)
    {
        Id = Guid.NewGuid();
        BlogPostId = Guard.Against.Default(blogPostId);
        UserId = Guard.Against.Default(userId);
        SetContent(content);
        MarkAsCreated(userId);
    }
    // Methods
    public void AddReply(BlogPostComment reply)
    {
        Guard.Against.Null(reply);
        _replies.Add(reply);
    }

    public void AddMention(Guid userId)
    {
        Guard.Against.Default(userId);
        if (!_mentions.Contains(userId))
        {
            _mentions.Add(userId);
        }
    }

    public void SetContent(string content)
        => Content = Guard.Against.NullOrWhiteSpace(content);

    public int GetRepliesCount()
        => _replies.SelectMany(reply => reply.Replies).Count();
}
