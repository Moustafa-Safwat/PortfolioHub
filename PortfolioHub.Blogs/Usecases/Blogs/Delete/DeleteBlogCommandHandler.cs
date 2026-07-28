using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Blogs.Delete;

internal sealed class DeleteBlogCommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<DeleteBlogCommand>
{
    public async Task<Result> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingBlogResult = await blogsRepo.GetByIdAsync(request.BlogId, cancellationToken);
            if (!existingBlogResult.IsSuccess)
                return existingBlogResult.PropagateFailure();

            // Implement hard delete, not soft delete
            var deleteResult = await blogsRepo.DeleteAsync(request.BlogId, cancellationToken);
            if (!deleteResult.IsSuccess)
                return deleteResult.PropagateFailure();

            var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
            if (!saveResult.IsSuccess)
                return saveResult.PropagateFailure();
        }
        catch (Exception ex)
        {
            var errorList = new ErrorList(
              [$"Failed to delete blog with id {request.BlogId}",
                ex.Message,
                ex?.InnerException?.Message??string.Empty]);
            return Result.Error(errorList);
        }

        return Result.SuccessWithMessage($"Blog with id {request.BlogId} is deleted successfully");
    }
}