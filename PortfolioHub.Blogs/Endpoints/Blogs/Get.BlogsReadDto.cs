using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed record BlogsReadDto(
    Guid Id,
    string Title,
    string Slug,
    string CoverImageUrl,
    BlogStatus Status,
    string Description,
    DateTime? PublishedAt,
    int ReadTime,
    int LikesNo,
    int CommentsNo,
    int ViewNo,
    bool IsFeatured,
    IReadOnlyCollection<string> Tags);
