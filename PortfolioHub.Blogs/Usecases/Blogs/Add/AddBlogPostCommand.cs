using PortfolioHub.Blogs.Endpoints.Blogs;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Add;

internal sealed record AddBlogPostCommand
(
    AddBlogRequest Blog,
    Guid UserId
) : ICommand<Guid>;
