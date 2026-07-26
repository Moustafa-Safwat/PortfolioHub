using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Endpoints.BlogTags;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using System.Collections.ObjectModel;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Get;

internal sealed class GetBlogTagsQueryHandler
(
    IEntityRepo<BlogPostTag> blogTagRepo
) : IQueryHandler<GetBlogTagsQuery, ReadOnlyCollection<TagsDto>>
{
    public async Task<Result<ReadOnlyCollection<TagsDto>>> Handle(GetBlogTagsQuery request, CancellationToken cancellationToken)
    {
        var blogTagResult = await blogTagRepo.GetAllAsync(1, 100, cancellationToken);
        if (!blogTagResult.IsSuccess)
            return blogTagResult.PropagateFailure<IReadOnlyList<BlogPostTag>, ReadOnlyCollection<TagsDto>>();

        var tagsDtos = blogTagResult.Value
            .Select(tag => new TagsDto(tag.Id, tag.Name))
            .ToList()
            .AsReadOnly();

        return Result.Success(tagsDtos);
    }
}
