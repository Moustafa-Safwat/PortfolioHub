using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed record BlogDetailsDto(
    Guid Id,
    string Title,
    string CoverImageUrl,
    string Slug,
    string Description,
    bool IsFeatured,
    BlogStatus Status,
    DateTime? PublishedAt,
    int ReadTime,
    int LikesNo,
    int CommentsNo,
    int ViewsNo,
    bool IsUserLiked,
    IReadOnlyCollection<string> Tags,
    IReadOnlyCollection<BlogAuthorDto> Authors,
    IReadOnlyCollection<BlogPostReferenceIdDto> References,
    IReadOnlyCollection<BlogPostBlockIdDto> Blocks
);
