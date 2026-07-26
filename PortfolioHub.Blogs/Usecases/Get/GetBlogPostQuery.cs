using PortfolioHub.Blogs.Endpoints;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Get;

internal sealed record GetBlogPostQuery
(
    List<Guid> TagIds,
    string Search,
    int PageNumber,
    int PageSize,
    bool IsFeatured
) : IQuery<GetBlogsResponse>;
