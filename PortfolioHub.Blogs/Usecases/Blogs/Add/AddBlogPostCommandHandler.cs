
using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Add;

internal sealed class AddBlogPostCommandHandler
(
    IBlogsRepo blogsRepo,
    IEntityRepo<BlogPostTag> blogTagRepo
) : ICommandHandler<AddBlogPostCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddBlogPostCommand request, CancellationToken cancellationToken)
    {
        // Check the tags ids are right
        var tagValidationResult = await blogTagRepo.IsEntitiesIdValidAsync(request.Blog.TagIds, cancellationToken);
        if (!tagValidationResult.IsSuccess)
            return tagValidationResult.PropagateFailure<IReadOnlyList<BlogPostTag>, Guid>();

        // Create blog post object, then save it in db
        var blogPost = BlogPost.Factory.Create
            (
                request.UserId,
                request.Blog.Title,
                request.Blog.CoverImageUrl,
                request.Blog.slug,
                request.Blog.Description,
                BlogStatus.Draft, // By default the status is draft untill user change it
                request.Blog.IsFeatured,
                tagValidationResult.Value,
                request.Blog.References,
                request.Blog.Blocks
            );

        var addBlogResult = await blogsRepo.AddAsync(blogPost, cancellationToken);
        if (!addBlogResult.IsSuccess)
            return addBlogResult;

        var savedBlogResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!savedBlogResult.IsSuccess)
            return savedBlogResult;

        return Result.Success(blogPost.Id);
    }
}