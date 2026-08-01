namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed record DeleteCommentReq(Guid BlogId, Guid CommentId);
