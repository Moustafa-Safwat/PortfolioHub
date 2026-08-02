using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Like;

internal sealed class LikeBlogcommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<LikeBlogCommand>
{
    public async Task<Result> Handle(LikeBlogCommand request, CancellationToken cancellationToken)
    {
        var getBlogResult = await blogsRepo.GetByIdAsync(
            request.BlogId,
            query => query.Include(b => b.BlogPostLikes),
            cancellationToken);

        if (!getBlogResult.IsSuccess)
            return getBlogResult.PropagateFailure();

        var blog = getBlogResult.Value;
        if (blog.Status != BlogStatus.Published)
        {
            var error = new ErrorList(["Can't like un-published blog"]);
            return Result.Error(error);
        }

        blog.AddLike(request.UserId);

        var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.SuccessWithMessage($"User with id: {request.UserId} liked the blog with id: {request.BlogId}");
    }
}
