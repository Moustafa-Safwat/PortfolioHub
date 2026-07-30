using PortfolioHub.Blogs.Endpoints.Blogs;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.GetById;

internal sealed record GetBlogByIdQuery(
    Guid BlogId,
    Guid UserId
) : IQuery<BlogDetailsDto>;
