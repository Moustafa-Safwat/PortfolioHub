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
    public DateTime? PublishedAtUtc { get; private set; }
    public BlogStatus Status { get; private set; }
    public bool IsFeatured { get; private set; } = false;
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
        PublishedAtUtc = DateTime.UtcNow;
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
            // soft delete
            like.UnLike();
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
        Guard.Against.Null(tag);
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
    public void BlogView(Guid userId)
    {
        var blogView = _blogPostViews.FirstOrDefault(b => b.UserId == userId);
        if (blogView is null)
        {
            var blogViewToAdd = new BlogPostView(Id, userId, DateTime.UtcNow);
            _blogPostViews.Add(blogViewToAdd);
        }
        else
        {
            blogView.IncrementViewCount();
        }
    }
    public void AddReference(Guid userId, string url, string label)
    {
        // Can't add a reference with the same url
        var existingRefByUrl = _blogReferences.FirstOrDefault(b => b.Url == url);
        if (existingRefByUrl is not null)
            existingRefByUrl.MarkAsDeleted(userId);

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
    public Guid AddComment(Guid userId, string comment, Guid? parentCommentGuid)
    {
        var parentComment = _blogComments.FirstOrDefault(comment => comment.Id == parentCommentGuid);
        var newComment = new BlogPostComment(Id, userId, comment);
        if (parentCommentGuid is not null && parentComment is not null)
        {
            parentComment.AddReply(newComment);
        }
        else
        {
            _blogComments.Add(newComment);
        }
        return newComment.Id;
    }
    public void RemoveComment(Guid userId, Guid commentId)
    {
        var comment = _blogComments.FirstOrDefault(comment => comment.Id == commentId);
        if (comment is not null)
        {
            comment.MarkAsDeleted(userId);
            comment.Replies
                .SelectMany(reply => reply.Replies)
                .ToList()
                .ForEach(reply => reply.MarkAsDeleted(userId));
        }
    }
    public int CommentsCount()
    {
        return _blogComments.Count + _blogComments.Sum(comment => comment.GetRepliesCount());
    }
    // Setters
    public void SetFeatured(bool isFeatured)
        => IsFeatured = isFeatured;
    public void SetTitle(string title)
        => Title = Guard.Against.NullOrWhiteSpace(title);
    public void SetCoverImageUrl(string coverImageUrl)
        => CoverImageUrl = Guard.Against.NullOrWhiteSpace(coverImageUrl);
    public void SetSlug(string slug)
        => Slug = Guard.Against.NullOrWhiteSpace(slug);
    public void SetDescription(string description)
        => Description = Guard.Against.NullOrWhiteSpace(description);
    public void SetStatus(BlogStatus status)
        => Status = Guard.Against.EnumOutOfRange(status);
    // Update
    public void UpdateTags(IEnumerable<BlogPostTag> tagsToUpdate)
    {
        Guard.Against.Null(tagsToUpdate);

        var requestedTags = tagsToUpdate
            .Where(tag => tag is not null)
            .GroupBy(tag => tag.Id)
            .Select(group => group.First())
            .ToList();

        if (requestedTags.Any(tag => tag.Id == Guid.Empty))
            throw new ArgumentException("A tag cannot have an empty ID.", nameof(tagsToUpdate));

        var requestedTagIds = requestedTags
            .Select(tag => tag.Id)
            .ToHashSet();

        var currentTagIds = _blogPostTags
            .Select(tag => tag.Id)
            .ToHashSet();

        var tagsToRemove = _blogPostTags
            .Where(tag => !requestedTagIds.Contains(tag.Id))
            .ToList();

        var tagsToAdd = requestedTags
            .Where(tag => !currentTagIds.Contains(tag.Id))
            .ToList();

        if (tagsToRemove.Count == 0 && tagsToAdd.Count == 0)
            return;

        tagsToRemove.ForEach(tag => RemoveTag(tag));
        tagsToAdd.ForEach(tag => AddTag(tag));
    }
    public void UpdateReferences(IEnumerable<BlogPostReferenceIdDto> referencesToUpdate, Guid userId)
    {
        Guard.Against.Null(referencesToUpdate);
        Guard.Against.Default(userId);

        var existingBlogRefIds = _blogReferences.Select(blogRef => blogRef.Id);
        var blogRefToBeUpdated = referencesToUpdate.Select(b => b.Id);

        existingBlogRefIds
            .Except(blogRefToBeUpdated)
            .ToList()
            .ForEach(blogRefId => RemoveReference(userId, blogRefId));

        referencesToUpdate
            .ToList()
            .ForEach(refToUpdate =>
            {
                var exisitingBlogRef = _blogReferences
                    .FirstOrDefault(reference => reference.Id == refToUpdate.Id);
                if (exisitingBlogRef is not null)
                {
                    // existing blog => need to be updated
                    exisitingBlogRef.Update(refToUpdate.Url, refToUpdate.Label, userId);
                }
                else
                {
                    if (!existingBlogRefIds.Contains(refToUpdate.Id))
                    {
                        // new blog reference
                        AddReference(userId, refToUpdate.Url, refToUpdate.Label);
                    }
                    else
                    {
                        // removed blog reference
                        RemoveReference(userId, refToUpdate.Id);
                    }
                }
            });
    }
    public void UpdateBlogBlock(IEnumerable<BlogPostBlockIdDto> blocksToUpdate, Guid userId)
    {
        Guard.Against.Null(blocksToUpdate);
        Guard.Against.Default(userId);

        var requestedBlocks = blocksToUpdate
            .Select(b => new BlogPostBlockIdDto(
                b.Id,
                Guard.Against.NullOrWhiteSpace(b.Type).Trim(),
                Guard.Against.NullOrWhiteSpace(b.Text).Trim(),
                Guard.Against.Negative(b.Order),
                b.Url?.Trim(),
                b.FileName?.Trim(),
                b.MimeType?.Trim(),
                b.CodeTitle?.Trim(),
                b.CodeLanguage?.Trim(),
                b.TextAlign?.Trim()))
            .ToList();

        // Ensure there are no duplicate IDs for existing blocks
        var duplicateIds = requestedBlocks
            .Where(b => b.Id != Guid.Empty)
            .GroupBy(b => b.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateIds.Count > 0)
            throw new ArgumentException($"Duplicate block IDs were provided: {string.Join(", ", duplicateIds)}.", nameof(blocksToUpdate));

        // Ensure no duplicate order positions in the requested set
        var duplicateOrders = requestedBlocks
            .GroupBy(b => b.Order)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateOrders.Count > 0)
            throw new ArgumentException($"Duplicate block orders were provided: {string.Join(", ", duplicateOrders)}.", nameof(blocksToUpdate));

        var activeBlocks = _blogPostBlocks
            .Where(b => !b.IsDeleted)
            .ToList();

        var activeBlocksById = activeBlocks
            .ToDictionary(b => b.Id);

        var requestedExistingIds = requestedBlocks
            .Where(b => b.Id != Guid.Empty)
            .Select(b => b.Id)
            .ToHashSet();

        // Mark blocks that are no longer present as deleted
        var blocksToRemove = activeBlocks
            .Where(b => !requestedExistingIds.Contains(b.Id))
            .ToList();

        foreach (var block in blocksToRemove)
        {
            block.MarkAsDeleted(userId);
        }

        var existsingPostBlockIds = _blogPostBlocks.Select(b => b.Id);
        // Apply requested changes and add new blocks
        foreach (var requested in requestedBlocks)
        {
            if (!existsingPostBlockIds.Contains(requested.Id))
            {
                var newBlock = new BlogPostBlock(Id, userId, requested.Text, requested.Type, requested.Order);

                if (!string.IsNullOrWhiteSpace(requested.Url))
                    newBlock.SetUrl(requested.Url!);
                if (!string.IsNullOrWhiteSpace(requested.FileName))
                    newBlock.SetFileName(requested.FileName!);
                if (!string.IsNullOrWhiteSpace(requested.MimeType))
                    newBlock.SetMimeType(requested.MimeType!);
                if (!string.IsNullOrWhiteSpace(requested.CodeTitle))
                    newBlock.SetCodeTitle(requested.CodeTitle!);
                if (!string.IsNullOrWhiteSpace(requested.CodeLanguage))
                    newBlock.SetCodeLanguage(requested.CodeLanguage!);
                if (!string.IsNullOrWhiteSpace(requested.TextAlign))
                    newBlock.SetTextAlign(requested.TextAlign!);

                _blogPostBlocks.Add(newBlock);
                continue;
            }

            if (!activeBlocksById.TryGetValue(requested.Id, out var existingBlock))
                throw new InvalidOperationException($"Block '{requested.Id}' does not belong to this blog post.");

            var typeChanged = !string.Equals(existingBlock.BlockType, requested.Type, StringComparison.Ordinal);
            var textChanged = !string.Equals(existingBlock.Text, requested.Text, StringComparison.Ordinal);
            var orderChanged = existingBlock.Order != requested.Order;
            var urlChanged = !string.Equals(existingBlock.Url ?? string.Empty, requested.Url ?? string.Empty, StringComparison.Ordinal);
            var fileNameChanged = !string.Equals(existingBlock.FileName ?? string.Empty, requested.FileName ?? string.Empty, StringComparison.Ordinal);
            var mimeChanged = !string.Equals(existingBlock.MimeType ?? string.Empty, requested.MimeType ?? string.Empty, StringComparison.Ordinal);
            var codeTitleChanged = !string.Equals(existingBlock.CodeTitle ?? string.Empty, requested.CodeTitle ?? string.Empty, StringComparison.Ordinal);
            var codeLangChanged = !string.Equals(existingBlock.CodeLanguage ?? string.Empty, requested.CodeLanguage ?? string.Empty, StringComparison.Ordinal);
            var textAlignChanged = !string.Equals(existingBlock.TextAlign ?? string.Empty, requested.TextAlign ?? string.Empty, StringComparison.Ordinal);

            if (!typeChanged && !textChanged && !orderChanged && !urlChanged && !fileNameChanged && !mimeChanged && !codeTitleChanged && !codeLangChanged && !textAlignChanged)
                continue;

            if (typeChanged)
                existingBlock.SetBlockType(requested.Type);
            if (textChanged)
                existingBlock.SetText(requested.Text);
            if (orderChanged)
                existingBlock.SetOrder(requested.Order);
            if (urlChanged)
                existingBlock.SetUrl(requested.Url ?? string.Empty);
            if (fileNameChanged)
                existingBlock.SetFileName(requested.FileName ?? string.Empty);
            if (mimeChanged)
                existingBlock.SetMimeType(requested.MimeType ?? string.Empty);
            if (codeTitleChanged)
                existingBlock.SetCodeTitle(requested.CodeTitle ?? string.Empty);
            if (codeLangChanged)
                existingBlock.SetCodeLanguage(requested.CodeLanguage ?? string.Empty);
            if (textAlignChanged)
                existingBlock.SetTextAlign(requested.TextAlign ?? string.Empty);

            existingBlock.MarkAsUpdated(userId);
        }
    }
    public int GetReadTimeMinutes()
    {
        int averageWordsPerMinute = 200;

        if (!_blogPostBlocks.Any())
            return 0;

        Func<string, int> countWords = (string text)
            => text.Split(
                [' ', '\t', '\r', '\n'],
                StringSplitOptions.RemoveEmptyEntries).Length;

        var totalWords = _blogPostBlocks
            .Where(block =>
                !block.IsDeleted &&
                !string.IsNullOrWhiteSpace(block.Text) &&
                !block.BlockType.Contains("image", StringComparison.OrdinalIgnoreCase) &&
                !block.BlockType.Contains("file", StringComparison.OrdinalIgnoreCase))
            .Sum(block => countWords(block.Text));

        return totalWords == 0 ? 0 : (int)Math.Ceiling((double)totalWords / averageWordsPerMinute);
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
            bool isFeatured,
            IEnumerable<BlogPostTag> tags,
            IEnumerable<BlogPostReferenceDto> references,
            IEnumerable<BlogPostBlockDto> blocks)

        {
            var blogPost = new BlogPost();
            blogPost.Id = Guid.NewGuid();
            blogPost.SetTitle(title);
            blogPost.SetCoverImageUrl(coverImageUrl);
            blogPost.SetSlug(slug);
            blogPost.SetDescription(description);
            blogPost.SetStatus(status);
            blogPost.SetFeatured(isFeatured);
            blogPost.MarkAsCreated(userId);
            blogPost.AddAuthor(userId, BlogPostAuthorRole.Owner, userId);// Add the creator as the owner of the blog post
            references.ToList().ForEach(reference => blogPost.AddReference(userId, reference.Url, reference.Label));
            tags.ToList().ForEach(tag => blogPost.AddTag(tag));
            blocks.ToList().ForEach(block =>
            {
                var blogPostBlock = new BlogPostBlock(blogPost.Id, userId, block.Text, block.Type, block.Order);

                if (block.Url is not null)
                    blogPostBlock.SetUrl(block.Url);
                if (block.FileName is not null)
                    blogPostBlock.SetFileName(block.FileName);
                if (block.MimeType is not null)
                    blogPostBlock.SetFileName(block.MimeType);
                if (block.CodeTitle is not null)
                    blogPostBlock.SetCodeTitle(block.CodeTitle);
                if (block.CodeLanguage is not null)
                    blogPostBlock.SetCodeLanguage(block.CodeLanguage);
                if (block.TextAlign is not null)
                    blogPostBlock.SetTextAlign(block.TextAlign);

                blogPost._blogPostBlocks.Add(blogPostBlock);
            });
            return blogPost;
        }
    }
}
