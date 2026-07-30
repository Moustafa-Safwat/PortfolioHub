using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Add;

internal sealed record AddTagCommand
(
    string Name
) : ICommand<Guid>;
