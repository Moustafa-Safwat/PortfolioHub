using PortfolioHub.Blogs.Endpoints.Blogs;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Update;

internal sealed record UpdateBlogPostCommand
(
    UpdateBlogRequest Blog,
    Guid UserId,
    Guid BlogId
) : ICommand;
