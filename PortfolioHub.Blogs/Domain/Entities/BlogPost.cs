using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Entities;

internal sealed class BlogPost : DeletionEntity
{
    private readonly IList<BlogPostLike> _blogPostLikes = [];
    private readonly IList<BlogPostView> _blogPostViews = [];
    private readonly IList<BlogPostAuthor> _blogPostAuthors = [];
    private readonly IList<BlogPostTag> _blogPostTags = [];
    private readonly IList<BlogPostBlock> _blogPostBlocks = [];
    private readonly IList<BlogPostComment> _blogComments = [];
    private readonly IList<BlogPostReferences> _blogReferences = [];

    public string Title { get; private set; } = null!;
    public string CoverImageUrl { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime? PublishedAt { get; private set; }
    public BlogStatus Status { get; private set; }
    public IReadOnlyCollection<BlogPostLike> BlogPostLikes => _blogPostLikes.AsReadOnly();
    public IReadOnlyCollection<BlogPostView> BlogPostViews => _blogPostViews.AsReadOnly();
    public IReadOnlyCollection<BlogPostAuthor> BlogPostAuthors => _blogPostAuthors.AsReadOnly();
    public IReadOnlyCollection<BlogPostTag> BlogPostTags => _blogPostTags.AsReadOnly();
    public IReadOnlyCollection<BlogPostBlock> BlogPostBlocks => _blogPostBlocks.AsReadOnly();
    public IReadOnlyCollection<BlogPostComment> BlogComments => _blogComments.AsReadOnly();
    public IReadOnlyCollection<BlogPostReferences> BlogReferences => _blogReferences.AsReadOnly();
    // EF Constructor
    private BlogPost() { }
    // Methods
    public void Publish(Guid userId)
    {
        Status = BlogStatus.Published;
        PublishedAt = DateTime.UtcNow;
        MarkAsUpdated(userId);
    }
    public void Archive(Guid userId)
    {
        Status = BlogStatus.Archived;
        MarkAsUpdated(userId);
    }
    public void Delete(Guid userId)
    {
        MarkAsDeleted(userId);
    }
    public void AddLike(Guid userId)
    {
        // Add only like if the user hasn't liked the post yet
        if (!_blogPostLikes.Any(b => b.UserId == userId))
        {
            var blogPostLike = new BlogPostLike(Id, userId);
            _blogPostLikes.Add(blogPostLike);
        }
    }
    public void RemoveLike(Guid userId)
    {
        var like = _blogPostLikes.FirstOrDefault(b => b.UserId == userId);
        if (like is not null)
        {
            like.UnLike();
            _blogPostLikes.Remove(like);
        }
    }
    public void AddAuthor(Guid userId, BlogPostAuthorRole role, Guid addedByUserId)
    {
        if (!_blogPostAuthors.Any(b => b.UserId == userId))
        {
            var blogPostAuthor = new BlogPostAuthor(Id, userId, role, addedByUserId);
            _blogPostAuthors.Add(blogPostAuthor);
        }
    }
    public void RemoveAuthor(Guid userId)
    {
        Guard.Against.Default(userId);
        var author = _blogPostAuthors.FirstOrDefault(b => b.UserId == userId);
        if (author is not null)
        {
            author.MarkAsDeleted(userId);
            _blogPostAuthors.Remove(author);
        }
    }
    public void AddTag(BlogPostTag tag)
    {
        if (!_blogPostTags.Any(t => t.Name == tag.Name))
        {
            _blogPostTags.Add(tag);
        }
    }
    public void RemoveTag(BlogPostTag tag)
    {
        Guard.Against.Null(tag);
        if (_blogPostTags.Contains(tag))
        {
            _blogPostTags.Remove(tag);
        }
    }
    public void AddReference(Guid userId, string url, string label)
    {
        var reference = new BlogPostReferences(Id, userId, url, label);
        _blogReferences.Add(reference);
    }
    public void RemoveReference(Guid userId, Guid referenceId)
    {
        var reference = _blogReferences.FirstOrDefault(r => r.Id == referenceId);
        if (reference is not null)
        {
            reference.Delete(userId);
            _blogReferences.Remove(reference);
        }
    }
    // Factory
    internal class Factory
    {
        public static BlogPost Create(
            Guid userId,
            string title,
            string coverImageUrl,
            string slug,
            string description,
            BlogStatus status,
            IEnumerable<BlogPostTag> tags,
            IEnumerable<(string Url, string Label)> references,
            IEnumerable<BlogPostBlock> blocks)

        {
            var blogPost = new BlogPost();
            blogPost.Id = Guid.NewGuid();
            blogPost.Title = Guard.Against.NullOrWhiteSpace(title);
            blogPost.CoverImageUrl = Guard.Against.NullOrWhiteSpace(coverImageUrl);
            blogPost.Slug = Guard.Against.NullOrWhiteSpace(slug);
            blogPost.Description = Guard.Against.NullOrWhiteSpace(description);
            blogPost.Status = Guard.Against.EnumOutOfRange(status);
            blogPost.MarkAsCreated(userId);
            blogPost.AddAuthor(userId, BlogPostAuthorRole.Owner, userId);// Add the creator as the owner of the blog post
            references.ToList().ForEach(reference => blogPost.AddReference(userId, reference.Url, reference.Label));
            tags.ToList().ForEach(tag => blogPost.AddTag(tag));
            blocks.ToList().ForEach(block => blogPost._blogPostBlocks.Add(block));
            return blogPost;
        }
    }
}
