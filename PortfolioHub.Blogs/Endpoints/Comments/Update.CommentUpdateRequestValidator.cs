using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class CommentUpdateRequestValidator
    : Validator<CommentUpdateRequest>
{
    private const int MaxCommentLength = 10_000;

    public CommentUpdateRequestValidator()
    {
        RuleFor(request => request.BlogId)
            .NotEqual(Guid.Empty)
            .WithMessage("Blog ID is required and must be a valid GUID.");

        RuleFor(request => request.CommentId)
            .NotEqual(Guid.Empty)
            .WithMessage("Comment ID is required and must be a valid GUID.");

        RuleFor(request => request.Comment)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Comment is required.")
            .Must(comment => !string.IsNullOrWhiteSpace(comment))
            .WithMessage("Comment cannot contain only whitespace.")
            .MaximumLength(MaxCommentLength)
            .WithMessage(
                $"Comment must not exceed {MaxCommentLength} characters.");
    }
}
