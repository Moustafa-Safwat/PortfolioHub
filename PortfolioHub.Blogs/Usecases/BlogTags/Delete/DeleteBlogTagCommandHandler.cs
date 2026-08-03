using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Delete;

internal sealed class DeleteBlogTagCommandHandler
(
    IEntityRepo<BlogPostTag> blogTagRepo
) : ICommandHandler<DeleteBlogTagCommand>
{
    public async Task<Result> Handle(DeleteBlogTagCommand request, CancellationToken cancellationToken)
    {
        var getBlogTagResult = await blogTagRepo.GetByIdAsync(request.BlogTagId, cancellationToken);
        if (!getBlogTagResult.IsSuccess)
            return getBlogTagResult.PropagateFailure();

        var deleteResult = await blogTagRepo.DeleteAsync(request.BlogTagId, cancellationToken);
        if (!deleteResult.IsSuccess)
            return deleteResult;

        var saveResult = await blogTagRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.SuccessWithMessage($"Blog tag with id: {request.BlogTagId} is successfully deleted.");
    }
}