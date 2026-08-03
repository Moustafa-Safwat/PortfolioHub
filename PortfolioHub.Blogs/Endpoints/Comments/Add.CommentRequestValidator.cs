using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class CommentRequestValidator
    : Validator<CommentRequest>
{
    private const int MaxCommentLength = 10_000;

    public CommentRequestValidator()
    {
        RuleFor(request => request.BlogId)
            .NotEqual(Guid.Empty)
            .WithMessage("Blog ID is required.");

        RuleFor(request => request.Comment)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Comment is required.")
            .Must(comment => !string.IsNullOrWhiteSpace(comment))
            .WithMessage("Comment cannot contain only whitespace.")
            .MaximumLength(MaxCommentLength)
            .WithMessage(
                $"Comment must not exceed {MaxCommentLength} characters.");

        RuleFor(request => request.ParentCommentId)
            .Must(id => id is null || id != Guid.Empty)
            .WithMessage("Parent comment ID must be a valid GUID.");
    }
}
