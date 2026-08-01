using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Delete;

internal sealed class DeleteCommentCommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<DeleteCommentCommand>
{
    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var getBlogByIdResult = await blogsRepo.GetByIdAsync(request.BlogId, cancellationToken);
        if (!getBlogByIdResult.IsSuccess)
            return getBlogByIdResult.PropagateFailure();

        var blog = getBlogByIdResult.Value;
        var commnetToDelete = blog.BlogComments.FirstOrDefault(comment => comment.Id == request.CommentId);
        if (commnetToDelete is null)
            return Result.NotFound($"Blog comment with id: {request.CommentId} is not found for blog id: {request.BlogId}");

        commnetToDelete.MarkAsDeleted(request.UserId);

        var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.PropagateFailure();

        return Result.SuccessWithMessage($"Comment with id: {request.CommentId} is deleted successfully");
    }
}
