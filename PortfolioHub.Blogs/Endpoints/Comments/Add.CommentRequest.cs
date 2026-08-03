namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed record CommentRequest
(
    Guid BlogId,
    string Comment,
    Guid? ParentCommentId
);
