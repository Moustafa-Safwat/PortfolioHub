using FastEndpoints;
using FluentValidation;
using PortfolioHub.SharedKernal.Config;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed class AddProjectReqValidator : Validator<AddProjectReq>
{
    public AddProjectReqValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.LongDescription)
            .NotEmpty().WithMessage("Long description is required.")
            .MinimumLength(20).WithMessage("Long description must be at least 20 characters long.")
            .MaximumLength(5000).WithMessage("Long description must not exceed 5000 characters.");

        RuleFor(x => x.CreatedDate)
            .NotEmpty().WithMessage("Created date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("CategoryId must be a valid GUID.");

        RuleFor(x => x.VideoUrl)
            .Must(url => url!.BeAValidUrl()).WithMessage("Video URL must be a valid URL format.");

        RuleFor(x => x.CoverImageUrl)
            .Must(url => url!.BeAValidUrl()).WithMessage("Cover image URL must be a valid URL format.");

        RuleFor(x => x.ImagesUrls)
            .NotNull().WithMessage("Images URLs are required.")
            .Must(urls => urls.All(url => url!.BeAValidUrl())).WithMessage("All image URLs must be valid URL formats.");

        RuleFor(x => x.SkillsId)
            .NotNull().WithMessage("Skills are required.")
            .Must(ids => ids.Length > 0 && ids.All(id => Guid.TryParse(id, out _)))
            .WithMessage("At least one skill is required.");
        RuleFor(x => x.Links)
            .NotNull().WithMessage("Links are required.")
            .ForEach(linkRule => linkRule.SetValidator(new LinkValidator()));
    }
}


internal sealed class LinkValidator : Validator<AddLinksDto>
{
    public LinkValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("Provider is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("ProviderId must be a valid GUID.");

        RuleFor(x => x.Link)
            .NotEmpty().WithMessage("Link URL is required.")
            .Must(link => link.BeAValidUrl()).WithMessage("Link URL must be a valid URL format.");
    }


}