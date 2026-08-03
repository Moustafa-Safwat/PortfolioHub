using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Update;

internal sealed class UpdateBlogTagCommandHandler
(
    IEntityRepo<BlogPostTag> blogTagRepo
) : ICommandHandler<UpdateBlogTagCommand>
{
    public async Task<Result> Handle(UpdateBlogTagCommand request, CancellationToken cancellationToken)
    {
        var blogTagResult = await blogTagRepo.GetByIdAsync(request.Id, cancellationToken);
        if (!blogTagResult.IsSuccess)
            return blogTagResult.PropagateFailure();

        var blogTag = blogTagResult.Value;
        blogTag.SetName(request.Name);

        var updateResult = await blogTagRepo.UpdateAsync(blogTag, cancellationToken);
        if (!updateResult.IsSuccess)
            return updateResult;

        var saveResult = await blogTagRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.SuccessWithMessage($"Blog Tag wit id: {request.Id} is updated successfully.");
    }
}