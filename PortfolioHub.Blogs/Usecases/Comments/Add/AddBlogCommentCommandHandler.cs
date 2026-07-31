using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.SharedKernal.Config;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Add;

internal sealed class AddBlogCommentCommandHandler
(
    IBlogsRepo blogsRepo
) : ICommandHandler<AddBlogCommentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddBlogCommentCommand request, CancellationToken cancellationToken)
    {
        var getBlogByIdResult = await blogsRepo.GetByIdAsync(request.BlogId);
        if (!getBlogByIdResult.IsSuccess)
            return getBlogByIdResult.PropagateFailure();

        var blog = getBlogByIdResult.Value;

        var commentId = blog.AddComment(request.UserId, request.Comment, request?.ParentCommentId);

        var saveResult = await blogsRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult;

        return Result.Success(commentId);
    }
}
