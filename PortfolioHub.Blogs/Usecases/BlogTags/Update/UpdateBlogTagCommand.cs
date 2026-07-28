using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Update;

internal sealed record UpdateBlogTagCommand
(
    Guid Id,
    string Name
) : ICommand;
