using FastEndpoints;
using FluentValidation;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class UpdateBlogRequestValidator : Validator<UpdateBlogRequest>
{
    public UpdateBlogRequestValidator()
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

        RuleFor(x => x.Status)
            .Must(s => Enum.IsDefined(typeof(BlogStatus), s))
            .WithMessage("Invalid blog status.");

        RuleFor(x => x.TagIds)
            .NotNull().WithMessage("TagIds must be provided.")
            .NotEmpty().WithMessage("TagIds must be provided.")
            .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
            .WithMessage("All TagIds must be valid GUIDs.");

        RuleFor(x => x.CoverImageUrl)
            .NotEmpty().WithMessage("Cover image URL is required.");

        RuleFor(x => x.References)
            .NotNull().WithMessage("References are required.")
            .ForEach(refRule => refRule.SetValidator(new BlogPostReferenceIdDtoValidator()));

        RuleFor(x => x.Blocks)
            .NotNull().WithMessage("Blocks are required.")
            .ForEach(blockRule => blockRule.SetValidator(new BlogPostBlockIdDtoValidator()));

        // Cross-field validations: duplicates
        RuleFor(x => x).Custom((req, ctx) =>
        {
            if (req.References is not null)
            {
                var duplicateRefIds = req.References
                    .Where(r => r.Id != Guid.Empty)
                    .GroupBy(r => r.Id)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateRefIds.Count > 0)
                    ctx.AddFailure("References", $"Duplicate reference IDs were provided: {string.Join(", ", duplicateRefIds)}.");
            }

            if (req.Blocks is not null)
            {
                var duplicateBlockIds = req.Blocks
                    .Where(b => b.Id != Guid.Empty)
                    .GroupBy(b => b.Id)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateBlockIds.Count > 0)
                    ctx.AddFailure("Blocks", $"Duplicate block IDs were provided: {string.Join(", ", duplicateBlockIds)}.");

                var duplicateOrders = req.Blocks
                    .GroupBy(b => b.Order)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateOrders.Count > 0)
                    ctx.AddFailure("Blocks", $"Duplicate block orders were provided: {string.Join(", ", duplicateOrders)}.");
            }
        });
    }

    internal sealed class BlogPostReferenceIdDtoValidator : Validator<BlogPostReferenceIdDto>
    {
        public BlogPostReferenceIdDtoValidator()
        {
            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("Reference label is required.")
                .MaximumLength(250).WithMessage("Reference label must not exceed 250 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Reference URL is required.")
                .Must(url => url!.BeAValidUrl()).WithMessage("Reference URL must be a valid URL.");
        }
    }

    internal sealed class BlogPostBlockIdDtoValidator : Validator<BlogPostBlockIdDto>
    {
        public BlogPostBlockIdDtoValidator()
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
                .MaximumLength(50).WithMessage("Text align must not exceed 20 characters.");
        }
    }
}
