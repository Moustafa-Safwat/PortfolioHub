using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class DeleteCommentValidator : Validator<DeleteCommentReq>
{
    public DeleteCommentValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.BlogId)
            .NotEqual(Guid.Empty)
            .WithMessage("Blog ID is required and must be a valid GUID.");

        RuleFor(request => request.CommentId)
            .NotEqual(Guid.Empty)
            .WithMessage("Comment ID is required and must be a valid GUID.");

        RuleFor(request => request)
            .Must(request => request.BlogId != request.CommentId)
            .WithMessage("Blog ID and comment ID cannot have the same value.");
    }
}
