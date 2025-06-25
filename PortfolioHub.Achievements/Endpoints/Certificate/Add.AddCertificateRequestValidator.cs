using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Achievements.Endpoints.Certificate;

internal sealed class AddCertificateRequestValidator : Validator<AddCertificateRequest>
{
    public AddCertificateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
        RuleFor(x => x.Issuer)
            .NotEmpty().WithMessage("Issuer is required.")
            .MaximumLength(200).WithMessage("Issuer must not exceed 200 characters.");
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date cannot be in the future.");
    }
}
