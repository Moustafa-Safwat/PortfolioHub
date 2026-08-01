using PortfolioHub.Blogs.Endpoints.Blogs;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.GetBySlug;

internal sealed record GetBlogBySlugQuery
(
    string Slug,
    Guid UserId
) : IQuery<BlogDetailsDto>;
