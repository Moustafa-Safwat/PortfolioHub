using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Publish;

internal sealed record PublishBlogCommand
(
    Guid BlogId,
    Guid UserId
) : ICommand;
