using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Delete;

internal sealed record DeleteBlogTagCommand
(
    Guid BlogTagId
) : ICommand;
