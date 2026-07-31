namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed record CommentUpdateRequest
(
    Guid BlogId,
    Guid CommentId,
    string Comment
);
