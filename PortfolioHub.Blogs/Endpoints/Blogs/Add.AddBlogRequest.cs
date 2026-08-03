using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed record AddBlogRequest
(
    string Title,
    string Description,
    string slug,
    List<Guid> TagIds,
    string CoverImageUrl,
    bool IsFeatured,
    List<BlogPostReferenceDto> References,
    List<BlogPostBlockDto> Blocks
);
