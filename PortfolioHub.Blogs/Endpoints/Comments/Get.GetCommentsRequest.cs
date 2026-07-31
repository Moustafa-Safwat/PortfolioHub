namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed record GetCommentsRequest
(
    Guid BlogId,
    int Page = 1,
    int PageSize = 10
);
