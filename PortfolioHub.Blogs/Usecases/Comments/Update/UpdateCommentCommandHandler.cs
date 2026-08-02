using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Update;

internal sealed class UpdateCommentCommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<UpdateCommentCommand>
{
    public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var getBlogByIdResult = await blogsRepo.GetByIdAsync(
            request.BlogId,
            query => query.Include(b => b.BlogComments),
            cancellationToken);

        if (!getBlogByIdResult.IsSuccess)
            return getBlogByIdResult.PropagateFailure();

        var blog = getBlogByIdResult.Value;

        var blocCommentToUpdate = blog.BlogComments?.FirstOrDefault(comment => comment.Id == request.CommentId);
        if (blocCommentToUpdate is null)
            return Result.NotFound($"Comment is not found with id: {request.Comment}");

        if (blocCommentToUpdate.UserId != request.UserId)
        {
            var error = new ErrorList(["Only the comment author can edit this comment"]);
            return Result.Error(error);
        }

        blocCommentToUpdate.SetContent(request.Comment);
        blocCommentToUpdate.MarkAsUpdated(request.UserId);

        var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.SuccessWithMessage($"Comment with id: {request.CommentId} is updated successfully");
    }
}
