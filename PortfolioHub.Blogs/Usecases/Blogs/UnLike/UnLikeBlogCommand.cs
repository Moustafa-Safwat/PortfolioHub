using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.UnLike;

internal sealed record UnLikeBlogCommand
(
    Guid BlogId,
    Guid UserId
) : ICommand;
