using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.Blogs.Endpoints.Blogs;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Get;

internal sealed class GetBlogPostQueryHandler
(
    IBlogsRepo blogsRepo
) : IQueryHandler<GetBlogPostQuery, GetBlogsResponse>
{
    public async Task<Result<GetBlogsResponse>> Handle(GetBlogPostQuery request, CancellationToken cancellationToken)
    {
        var blogsResult = await blogsRepo.GetAllAsync(
            request.PageNumber,
            request.PageSize,
            request.TagIds,
            request.Search,
            request.UserId,
            request.IsFeatured,
            cancellationToken);

        if (!blogsResult.IsSuccess)
        {
            return blogsResult.PropagateFailure<IReadOnlyList<BlogPost>, GetBlogsResponse>();
        }

        var blogsReadDto = blogsResult.Value
            .ToList()
            .Select(blog =>
            {
                return new BlogsReadDto(
                    blog.Id,
                    blog.Title,
                    blog.Slug,
                    blog.CoverImageUrl,
                    blog.Status,
                    blog.Description,
                    blog.PublishedAtUtc,
                    blog.GetReadTimeMinutes(),
                    blog.BlogPostLikes.Where(b => b.IsLiked).Count(),
                    blog.BlogComments.Count,
                    blog.BlogPostViews.Count,
                    blog.IsFeatured,
                    blog!.BlogPostTags
                        .Select(tag => tag.Name)
                        .ToList()
                        .AsReadOnly());
            }).ToList()
            .AsReadOnly();

        var blogResponse = new GetBlogsResponse(
            blogsReadDto,
            blogsReadDto.Count,
            request.PageNumber,
            request.PageSize);

        return Result.Success(blogResponse);
    }
}
