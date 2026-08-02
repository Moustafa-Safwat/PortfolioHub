using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Publish;

internal sealed class PublishBlogCommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<PublishBlogCommand>
{
    public async Task<Result> Handle(PublishBlogCommand request, CancellationToken cancellationToken)
    {
        var getByIdResult = await blogsRepo.GetByIdAsync(
            request.BlogId,
            null!, // Assuming no specific includes are needed
            cancellationToken);

        if (!getByIdResult.IsSuccess)
            return getByIdResult.PropagateFailure();

        var blog = getByIdResult.Value;
        blog.Publish(request.UserId);

        var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.SuccessWithMessage($"Blog with id: {request.BlogId} has been published successfully.");
    }
}
