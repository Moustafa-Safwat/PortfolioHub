using PortfolioHub.Blogs.Endpoints.Blogs;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Get;

internal sealed record GetBlogPostQuery
(
    List<Guid> TagIds,
    string Search,
    int PageNumber,
    int PageSize,
    bool IsFeatured,
    Guid UserId
) : IQuery<GetBlogsResponse>;
