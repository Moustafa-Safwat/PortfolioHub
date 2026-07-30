using PortfolioHub.Blogs.Endpoints.BlogTags;
using System.Collections.ObjectModel;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Get;

internal sealed record GetBlogTagsQuery()
    : IQuery<ReadOnlyCollection<TagsDto>>;
