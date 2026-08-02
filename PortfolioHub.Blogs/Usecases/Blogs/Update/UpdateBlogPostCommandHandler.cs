using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Update;

internal sealed class UpdateBlogPostCommandHandler(
    IBlogsRepo blogsRepo,
    IEntityRepo<BlogPostTag> blogTagRepo
) : ICommandHandler<UpdateBlogPostCommand>
{
    public async Task<Result> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // validate tags
            var tagValidationResult = await blogTagRepo.IsEntitiesIdValidAsync(request.Blog.TagIds, cancellationToken);
            if (!tagValidationResult.IsSuccess)
                return tagValidationResult.PropagateFailure();
            // get existing blog to update
            var existingBlogResult = await blogsRepo.GetByIdAsync(request.BlogId, blogsRepo.IncludeAll, cancellationToken);
            if (!existingBlogResult.IsSuccess)
                return existingBlogResult.PropagateFailure();

            var existingBlog = existingBlogResult.Value;
            // Update the main properties
            existingBlog.SetTitle(request.Blog.Title);
            existingBlog.SetStatus((BlogStatus)request.Blog.Status);
            existingBlog.SetCoverImageUrl(request.Blog.CoverImageUrl);
            existingBlog.SetDescription(request.Blog.Description);
            existingBlog.SetFeatured(request.Blog.IsFeatured);
            existingBlog.SetSlug(request.Blog.slug);
            // mark as updated
            existingBlog.UpdateTags(tagValidationResult.Value);
            existingBlog.UpdateReferences(request.Blog.References, request.UserId);
            existingBlog.UpdateBlogBlock(request.Blog.Blocks, request.UserId);
            existingBlog.MarkAsUpdated(request.UserId);

            var updateResult = await blogsRepo.UpdateAsync(existingBlog, cancellationToken);
            if (!updateResult.IsSuccess)
                return updateResult;

            var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
            if (!saveResult.IsSuccess)
                return saveResult;
        }
        catch (Exception ex)
        {
            var errorList = new ErrorList(
                [$"Failed to update blog with id {request.BlogId}",
                ex.Message,
                ex?.InnerException?.Message??string.Empty]);
            return Result.Error(errorList);
        }

        return Result.SuccessWithMessage("Blog is updated successfully");
    }
}
