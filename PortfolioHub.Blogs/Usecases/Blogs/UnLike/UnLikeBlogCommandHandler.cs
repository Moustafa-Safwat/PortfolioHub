using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.UnLike;

internal sealed class UnLikeBlogCommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<UnLikeBlogCommand>
{
    public async Task<Result> Handle(UnLikeBlogCommand request, CancellationToken cancellationToken)
    {
        var getBlogResult = await blogsRepo.GetByIdAsync(request.BlogId, cancellationToken);
        if (!getBlogResult.IsSuccess)
            return getBlogResult.PropagateFailure();

        var blog = getBlogResult.Value;
        if (blog.Status != Domain.Entities.BlogStatus.Published)
        {
            var error = new ErrorList(["Can't unlike un-published blog"]);
            return Result.Error(error);
        }

        blog.RemoveLike(request.UserId);

        var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.SuccessWithMessage($"User with id: {request.UserId} unliked blog with id: {request.BlogId}");
    }
}
