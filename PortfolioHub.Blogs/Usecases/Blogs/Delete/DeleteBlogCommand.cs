using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Delete;

internal sealed record DeleteBlogCommand
(
    Guid BlogId,
    Guid UserId
) : ICommand;
