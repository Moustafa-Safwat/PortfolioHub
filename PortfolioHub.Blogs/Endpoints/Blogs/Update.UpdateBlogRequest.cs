using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed record UpdateBlogRequest
(
    string Title,
    string Description,
    string slug,
    int Status,
    List<Guid> TagIds,
    string CoverImageUrl,
    bool IsFeatured,
    List<BlogPostReferenceIdDto> References,
    List<BlogPostBlockIdDto> Blocks
);
