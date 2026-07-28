using FastEndpoints;
using FluentValidation;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class AddBlogRequestValidator : Validator<AddBlogRequest>
{
    public AddBlogRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
            .MaximumLength(5000).WithMessage("Description must not exceed 5000 characters.");

        RuleFor(x => x.slug)
            .NotEmpty().WithMessage("Slug is required.")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase letters, numbers and hyphens only.")
            .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

        RuleFor(x => x.TagIds)
            .NotNull().WithMessage("TagIds must be provided.")
            .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
            .WithMessage("All TagIds must be valid GUIDs.");

        RuleFor(x => x.CoverImageUrl)
            .NotEmpty().WithMessage("Cover image URL is required.");

        RuleFor(x => x.References)
            .NotNull().WithMessage("References are required.")
            .ForEach(refRule => refRule.SetValidator(new BlogPostReferenceDtoValidator()));

        RuleFor(x => x.Blocks)
            .NotNull().WithMessage("Blocks are required.")
            .ForEach(blockRule => blockRule.SetValidator(new BlogPostBlockDtoValidator()));
    }

    internal sealed class BlogPostReferenceDtoValidator : Validator<BlogPostReferenceDto>
    {
        public BlogPostReferenceDtoValidator()
        {
            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("Reference label is required.")
                .MaximumLength(250).WithMessage("Reference label must not exceed 250 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Reference URL is required.")
                .Must(url => url!.BeAValidUrl()).WithMessage("Reference URL must be a valid URL.");
        }
    }

    internal sealed class BlogPostBlockDtoValidator : Validator<BlogPostBlockDto>
    {
        public BlogPostBlockDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Block type is required.")
                .MaximumLength(50).WithMessage("Block type must not exceed 50 characters.");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Block text is required.");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Block order must be zero or greater.");

            RuleFor(x => x.MimeType)
                .MaximumLength(150).WithMessage("Mime type must not exceed 150 characters.");

            RuleFor(x => x.TextAlign)
                .MaximumLength(20).WithMessage("Text align must not exceed 20 characters.");
        }
    }
}
