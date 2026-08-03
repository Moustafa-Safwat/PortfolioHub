using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Delete;

internal sealed record DeleteCommentCommand
(
    Guid BlogId,
    Guid CommentId,
    Guid UserId
) : ICommand;
