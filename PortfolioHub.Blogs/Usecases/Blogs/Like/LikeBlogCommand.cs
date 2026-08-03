using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Like;

internal sealed record LikeBlogCommand
(
    Guid BlogId,
    Guid UserId
) : ICommand;
