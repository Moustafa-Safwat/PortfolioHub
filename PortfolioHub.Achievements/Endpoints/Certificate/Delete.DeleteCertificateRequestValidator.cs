using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Achievements.Endpoints.Certificate;

internal sealed class DeleteCertificateRequestValidator : Validator<DeleteCertificateRequest>
{
    public DeleteCertificateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certificate ID must not be empty.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Certificate ID must be a valid GUID.");
    }
}
