using PortfolioHub.Blogs.Endpoints.Comments;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Get;

internal sealed record GetBlogCommentsQuery
(
    Guid BlogId,
    Guid UserId,
    int PageNumber,
    int PageSize
) : IQuery<IReadOnlyCollection<GetCommentsResponse>>;
