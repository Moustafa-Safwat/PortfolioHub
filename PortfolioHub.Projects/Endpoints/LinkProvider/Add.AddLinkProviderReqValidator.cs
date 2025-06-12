using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.LinkProvider;

internal class AddLinkProviderReqValidator : FastEndpoints.Validator<AddLinkProviderReq>
{
    public AddLinkProviderReqValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.BaseUrl)
            .NotEmpty().WithMessage("URL is required.")
            .Must(BeAValidUrl).WithMessage("Invalid URL format.");
    }
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
