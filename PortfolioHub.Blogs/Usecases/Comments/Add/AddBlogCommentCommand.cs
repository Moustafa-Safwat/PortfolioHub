using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Add;

internal sealed record AddBlogCommentCommand
(
    Guid BlogId,
    Guid UserId,
    string Comment,
    Guid? ParentCommentId
) : ICommand<Guid>;
