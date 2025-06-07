using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed class AddGalleryReqValidator : Validator<AddGalleryReq>
{
    public AddGalleryReqValidator()
    {
        RuleFor(t => t.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                         (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            .WithMessage("Image URL must be a valid HTTP or HTTPS URL.");

        RuleFor(t => t.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Order must be a non-negative integer.");
    }
}
