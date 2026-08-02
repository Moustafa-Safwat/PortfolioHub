using Ardalis.Result;
using MediatR;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.Blogs.Endpoints.Blogs;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.Users;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.GetBySlug;

internal sealed class GetBlogBySlugQueryHandler
(
    IBlogsRepo blogsRepo,
    ISender sender
) : IQueryHandler<GetBlogBySlugQuery, BlogDetailsDto>
{
    public async Task<Result<BlogDetailsDto>> Handle(GetBlogBySlugQuery request, CancellationToken cancellationToken)
    {
        var blogRes = await blogsRepo.GetBySlugAsync(request.Slug, cancellationToken);
        if (!blogRes.IsSuccess)
            return blogRes.PropagateFailure<BlogPost, BlogDetailsDto>();
        var blog = blogRes.Value;
        if (blog.Status != BlogStatus.Published)
        {
            if (request.UserId == Guid.Empty || !blog.BlogPostAuthors.Any(a => a.UserId == request.UserId))
                return Result<BlogDetailsDto>.NotFound("Blog not found");
        }
        var authorIds = blog.BlogPostAuthors.Select(a => a.UserId);
        var getUsersQuery = new GetUsersByIdQuery(authorIds);
        var getUsersDataResult = await sender.Send(getUsersQuery);
        if (!getUsersDataResult.IsSuccess)
            return getUsersDataResult.PropagateFailure<IEnumerable<GetUserDto>, BlogDetailsDto>();
        var getUsersData = getUsersDataResult.Value;
        if (blog.Status == BlogStatus.Published)
        {
            // Count views in case of published blogs
            blog.BlogView(request.UserId);
            var viewResult = await blogsRepo.SaveChangesAsync(cancellationToken);
            if (!viewResult.IsSuccess) return viewResult;
        }
        var dto = new BlogDetailsDto(
            blog.Id,
            blog.Title,
            blog.CoverImageUrl,
            blog.Slug,
            blog.Description,
            blog.IsFeatured,
            blog.Status,
            blog.PublishedAtUtc,
            blog.GetReadTimeMinutes(),
            blog.BlogPostLikes.Count,
            blog.BlogComments.Count,
            blog.BlogPostViews.Sum(b => b.ViewCount),
            blog.BlogPostLikes.Any(b => b.UserId == request.UserId),
            blog.BlogPostTags
                .Select(t => t.Name)
                .ToList()
                .AsReadOnly(),
            blog.BlogPostAuthors
                .Select(a =>
                {
                    var user = getUsersData.FirstOrDefault(user => user.Id == a.UserId);
                    // user should be with value all the time
                    return new BlogAuthorDto(user!.Id, user.FirstName, user.LastName, user.Email, a.Role);
                })
                .ToList()
                .AsReadOnly(),
            blog.BlogReferences
                .Where(r => !r.IsDeleted)
                .Select(r => new BlogPostReferenceIdDto(r.Id, r.Label, r.Url))
                .ToList()
                .AsReadOnly(),
            blog.BlogPostBlocks
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.Order)
                .Select(b => new BlogPostBlockIdDto(b.Id, b.BlockType, b.Text, b.Order, b.Url, b.FileName, b.MimeType, b.CodeTitle, b.CodeLanguage, b.TextAlign))
                .ToList()
                .AsReadOnly());

        return Result.Success(dto);
    }
}