using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Update;

internal sealed record UpdateCommentCommand
(
    Guid BlogId,
    Guid UserId,
    Guid CommentId,
    string Comment
) : ICommand;
